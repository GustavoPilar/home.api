namespace home.api.Exceptions
{
    /// <summary>
    /// Lançada quando a persistência não conclui a operação solicitada
    /// </summary>
    public class PersistenceException : Exception
    {
        #region Constructors

        public PersistenceException(string message) : base(message)
        {
        }

        public PersistenceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
