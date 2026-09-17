using home.api.Domain.Entities;
using home.api.Domain.Interfaces.Repositories;
using home.api.Infra.Db;
using Microsoft.EntityFrameworkCore;

namespace home.api.Infra.Repositories
{
    /// <summary>
    /// Acesso a dados da família, sempre resolvido pela associação UserFamily
    /// </summary>
    public class FamilyRepository(
        AppDbContext context) : IFamilyRepository
    {
        #region Fields

        private readonly AppDbContext context = context;
        private readonly DbSet<Family> families = context.Set<Family>();
        private readonly DbSet<UserFamily> members = context.Set<UserFamily>();
        private readonly DbSet<FamilyTitle> titles = context.Set<FamilyTitle>();

        #endregion

        #region Members :: Família

        /// <summary>
        /// Lista as famílias das quais o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        public async Task<IEnumerable<Family>> GetFamiliesByMemberAsync(Guid userId)
        {
            return await this.families
                .AsNoTracking()
                .Include(x => x.UserFamilies!)
                    .ThenInclude(m => m.FamilyTitle)
                .Where(x => x.UserFamilies!.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        /// <summary>
        /// Busca a família apenas se o usuário for membro dela
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        public async Task<Family?> GetByIdForMemberAsync(Guid userId, Guid familyId)
        {
            return await this.families
                .Include(x => x.UserFamilies!)
                    .ThenInclude(m => m.FamilyTitle)
                .FirstOrDefaultAsync(x => x.Id == familyId && x.UserFamilies!.Any(m => m.UserId == userId));
        }

        public void AddFamily(Family family)
        {
            ArgumentNullException.ThrowIfNull(family);

            this.families.Add(family);
        }

        public void DeleteFamily(Family family)
        {
            ArgumentNullException.ThrowIfNull(family);

            this.families.Remove(family);
        }

        #endregion

        #region Members :: Associações

        /// <summary>
        /// Busca a associação do usuário com a família
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        public async Task<UserFamily?> GetMembershipAsync(Guid userId, Guid familyId)
        {
            return await this.members
                .FirstOrDefaultAsync(x => x.UserId == userId && x.FamilyId == familyId);
        }

        /// <summary>
        /// Lista as associações de uma família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        public async Task<IEnumerable<UserFamily>> GetMembersAsync(Guid familyId)
        {
            return await this.members
                .Include(x => x.FamilyTitle)
                .Where(x => x.FamilyId == familyId)
                .ToListAsync();
        }

        public void AddMember(UserFamily member)
        {
            ArgumentNullException.ThrowIfNull(member);

            this.members.Add(member);
        }

        public void RemoveMember(UserFamily member)
        {
            ArgumentNullException.ThrowIfNull(member);

            this.members.Remove(member);
        }

        #endregion

        #region Members :: Títulos

        /// <summary>
        /// Lista os títulos globais mais os da própria família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        public async Task<IEnumerable<FamilyTitle>> GetTitlesAsync(Guid familyId)
        {
            return await this.titles
                .AsNoTracking()
                .Where(x => x.FamilyId == null || x.FamilyId == familyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Busca um título pelo identificador
        /// </summary>
        /// <param name="titleId">Título ID</param>
        public async Task<FamilyTitle?> GetTitleByIdAsync(Guid titleId)
        {
            return await this.titles
                .FirstOrDefaultAsync(x => x.Id == titleId);
        }

        /// <summary>
        /// Indica se a família já possui um título com esse nome
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="name">Nome do título</param>
        /// <param name="ignoreTitleId">Título a desconsiderar, usado na atualização</param>
        public async Task<bool> TitleNameExistsAsync(Guid familyId, string name, Guid? ignoreTitleId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            // Compara também com os globais: não faz sentido a família duplicar "Pai"
            return await this.titles
                .AsNoTracking()
                .AnyAsync(x =>
                    (x.FamilyId == null || x.FamilyId == familyId)
                    && x.Name.ToUpper() == name.ToUpper()
                    && (ignoreTitleId == null || x.Id != ignoreTitleId));
        }

        /// <summary>
        /// Verifica se todos os títulos informados são globais ou da própria família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleIds">Títulos referenciados</param>
        public async Task<bool> TitlesAreAvailableAsync(Guid familyId, IEnumerable<Guid> titleIds)
        {
            ArgumentNullException.ThrowIfNull(titleIds);

            List<Guid> ids = titleIds.Distinct().ToList();

            if (ids.Count == 0)
                return true;

            int available = await this.titles
                .AsNoTracking()
                .CountAsync(x => ids.Contains(x.Id) && (x.FamilyId == null || x.FamilyId == familyId));

            return available == ids.Count;
        }

        public void AddTitle(FamilyTitle title)
        {
            ArgumentNullException.ThrowIfNull(title);

            this.titles.Add(title);
        }

        public void RemoveTitle(FamilyTitle title)
        {
            ArgumentNullException.ThrowIfNull(title);

            this.titles.Remove(title);
        }

        #endregion
    }
}
