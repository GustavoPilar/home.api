using home.api.Domain.Interfaces.Entities;

namespace home.api.Domain.Entities.Base
{
    /// <summary>
    /// Base de identidade e auditoria, sem vínculo com usuário
    /// </summary>
    public abstract class EntityBase : IEntityBase
    {
        #region Properties

        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data da última alteração
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
