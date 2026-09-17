namespace home.api.Domain.Interfaces.Entities
{
    /// <summary>
    /// Identidade e auditoria comuns a toda entidade de domínio
    /// </summary>
    public interface IEntityBase
    {
        #region Properties

        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
