namespace home.api.Exceptions
{
    /// <summary>
    /// Lançada quando a entidade não existe ou não pertence ao usuário autenticado
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        #region Constructors

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
