using home.api.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Título de parentesco atribuível a um membro.
    /// Com FamilyId nulo é um título global, disponível para todas as famílias;
    /// preenchido, pertence somente à família que o criou.
    /// </summary>
    public class FamilyTitle : EntityBase
    {
        #region Properties

        /// <summary>
        /// Nome do título, como "Esposa" ou "Primogênito"
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Família dona do título. Nulo indica título global.
        /// </summary>
        public Guid? FamilyId { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public Family? Family { get; set; }

        [JsonIgnore]
        public ICollection<UserFamily>? UserFamilies { get; set; }

        #endregion
    }
}
