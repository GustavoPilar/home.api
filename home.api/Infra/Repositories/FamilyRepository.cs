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

        #endregion

        #region Members :: GetFamiliesByMemberAsync(), GetByIdForMemberAsync(), IsMemberAsync(), GetMembersAsync(), AddFamily(), DeleteFamily(), AddMember(), RemoveMember()

        /// <summary>
        /// Lista as famílias das quais o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        public async Task<IEnumerable<Family>> GetFamiliesByMemberAsync(Guid userId)
        {
            return await this.families
                .AsNoTracking()
                .Include(x => x.UserFamilies)
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
                .Include(x => x.UserFamilies)
                .FirstOrDefaultAsync(x => x.Id == familyId && x.UserFamilies!.Any(m => m.UserId == userId));
        }

        /// <summary>
        /// Indica se o usuário pertence à família
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        public async Task<bool> IsMemberAsync(Guid userId, Guid familyId)
        {
            return await this.members
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId && x.FamilyId == familyId);
        }

        /// <summary>
        /// Lista as associações de uma família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        public async Task<IEnumerable<UserFamily>> GetMembersAsync(Guid familyId)
        {
            return await this.members
                .Where(x => x.FamilyId == familyId)
                .ToListAsync();
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
    }
}
