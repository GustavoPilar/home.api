namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Saída da família. Quando quem sai é o único host, é obrigatório
    /// indicar o sucessor: a família nunca pode ficar sem administrador.
    /// </summary>
    public class FamilyLeaveRequest
    {
        #region Properties

        /// <summary>
        /// Membro que assumirá como host
        /// </summary>
        public Guid? NewHostId { get; set; }

        #endregion
    }
}
