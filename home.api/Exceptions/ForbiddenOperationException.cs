namespace home.api.Exceptions
{
    /// <summary>
    /// Lançada quando o usuário é membro, porém não tem permissão para a operação.
    /// Distingue-se de EntityNotFoundException: aqui o recurso existe e é visível,
    /// o que falta é autorização.
    /// </summary>
    public class ForbiddenOperationException : Exception
    {
        #region Constructors

        public ForbiddenOperationException(string message) : base(message)
        {
        }

        public ForbiddenOperationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
