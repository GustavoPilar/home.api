using home.api.Domain.Entities;

namespace home.api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Acesso a dados da família e do seu catálogo de títulos.
    /// Não usa o repositório genérico porque família não tem proprietário:
    /// todo acesso é resolvido pela associação UserFamily.
    /// </summary>
    public interface IFamilyRepository
    {
        #region Members :: Família

        /// <summary>
        /// Lista as famílias das quais o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        Task<IEnumerable<Family>> GetFamiliesByMemberAsync(Guid userId);

        /// <summary>
        /// Busca a família apenas se o usuário for membro dela
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        Task<Family?> GetByIdForMemberAsync(Guid userId, Guid familyId);

        void AddFamily(Family family);

        void DeleteFamily(Family family);

        #endregion

        #region Members :: Associações

        /// <summary>
        /// Busca a associação do usuário com a família, para conferir o papel
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        Task<UserFamily?> GetMembershipAsync(Guid userId, Guid familyId);

        /// <summary>
        /// Lista as associações de uma família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        Task<IEnumerable<UserFamily>> GetMembersAsync(Guid familyId);

        void AddMember(UserFamily member);

        void RemoveMember(UserFamily member);

        #endregion

        #region Members :: Títulos

        /// <summary>
        /// Lista os títulos disponíveis para a família: os globais mais os próprios
        /// </summary>
        /// <param name="familyId">Família ID</param>
        Task<IEnumerable<FamilyTitle>> GetTitlesAsync(Guid familyId);

        /// <summary>
        /// Busca um título pelo identificador
        /// </summary>
        /// <param name="titleId">Título ID</param>
        Task<FamilyTitle?> GetTitleByIdAsync(Guid titleId);

        /// <summary>
        /// Indica se a família já possui um título com esse nome
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="name">Nome do título</param>
        /// <param name="ignoreTitleId">Título a desconsiderar, usado na atualização</param>
        Task<bool> TitleNameExistsAsync(Guid familyId, string name, Guid? ignoreTitleId);

        /// <summary>
        /// Verifica se todos os títulos informados podem ser usados pela família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleIds">Títulos referenciados</param>
        Task<bool> TitlesAreAvailableAsync(Guid familyId, IEnumerable<Guid> titleIds);

        void AddTitle(FamilyTitle title);

        void RemoveTitle(FamilyTitle title);

        #endregion
    }
}
