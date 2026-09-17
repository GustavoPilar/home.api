using home.api.Domain.Entities;

namespace home.api.Domain.Interfaces.Entities
{
    /// <summary>
    /// Entidade que pertence a um único usuário.
    /// É esta interface, e não IEntityBase, que habilita o filtro por
    /// proprietário do repositório e do serviço genéricos — entidades
    /// compartilhadas, como Family, deliberadamente não a implementam.
    /// </summary>
    public interface IOwnedEntity : IEntityBase
    {
        #region Properties

        Guid UserId { get; set; }

        User? User { get; set; }

        #endregion
    }
}
