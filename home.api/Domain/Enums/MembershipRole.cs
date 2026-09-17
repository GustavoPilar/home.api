namespace home.api.Domain.Enums
{
    /// <summary>
    /// Papel de permissão do membro dentro da família.
    /// Conjunto fechado e controlado pela aplicação — diferente do título de
    /// parentesco, que é aberto e mantido pelo próprio host.
    /// </summary>
    public enum MembershipRole
    {
        /// <summary>
        /// Apenas leitura da família, dos membros e dos títulos
        /// </summary>
        Member = 0,

        /// <summary>
        /// Administra a família: membros, títulos, nome e exclusão
        /// </summary>
        Host = 1
    }
}
