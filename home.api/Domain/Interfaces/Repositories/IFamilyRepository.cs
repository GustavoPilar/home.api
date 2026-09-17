using home.api.Domain.Entities;

namespace home.api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Acesso a dados da família.
    /// Não usa o repositório genérico porque família não tem proprietário:
    /// todo acesso é resolvido pela associação UserFamily.
    /// </summary>
    public interface IFamilyRepository
    {
        #region Members :: GetFamiliesByMemberAsync(), GetByIdForMemberAsync(), IsMemberAsync(), GetMembersAsync(), AddFamily(), DeleteFamily(), AddMember(), RemoveMember()

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

        /// <summary>
        /// Indica se o usuário pertence à família
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        Task<bool> IsMemberAsync(Guid userId, Guid familyId);

        /// <summary>
        /// Lista as associações de uma família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        Task<IEnumerable<UserFamily>> GetMembersAsync(Guid familyId);

        void AddFamily(Family family);

        void DeleteFamily(Family family);

        void AddMember(UserFamily member);

        void RemoveMember(UserFamily member);

        #endregion
    }
}
