namespace home.api.Utilities
{
    /// <summary>
    /// Constantes compartilhadas pela aplicação
    /// </summary>
    public static class Global
    {
        #region Constants

        /// <summary>
        /// Texto livre: letras acentuadas, números, pontuação usual e apóstrofos.
        /// Fonte única para que criação e atualização validem o mesmo conjunto.
        /// </summary>
        public const string REGEX_DEFAULT = @"^[A-Za-zÀÁÂÃÇÉÊÍÓÔÕÚàáâãçéêíóôõú0-9'_’@.#$()\/,\-ºª° ]+$";

        /// <summary>
        /// Somente dígitos
        /// </summary>
        public const string REGEX_ONLY_NUMBERS = @"^[0-9]+$";

        /// <summary>
        /// Validade do convite de família, em horas
        /// </summary>
        public const int INVITE_EXPIRATION_IN_HOURS = 24;

        #endregion
    }
}
