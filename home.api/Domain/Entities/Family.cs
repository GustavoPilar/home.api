using home.api.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Família: entidade compartilhada, sem proprietário individual.
    /// O vínculo com os usuários é sempre através de UserFamily, e quem
    /// administra é identificado pelo papel Host nessa associação.
    /// </summary>
    public class Family : EntityBase
    {
        #region Properties

        /// <summary>
        /// Nome da família
        /// </summary>
        public string Name { get; set; } = string.Empty;

        #endregion

        #region Navigation

        /// <summary>
        /// Associações de membros da família
        /// </summary>
        [JsonIgnore]
        public ICollection<UserFamily>? UserFamilies { get; set; }

        /// <summary>
        /// Títulos criados por esta família
        /// </summary>
        [JsonIgnore]
        public ICollection<FamilyTitle>? FamilyTitles { get; set; }

        /// <summary>
        /// Lares compartilhados com a família
        /// </summary>
        [JsonIgnore]
        public ICollection<Home>? Homes { get; set; }

        #endregion
    }
}
