using home.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace home.api.Infra.Configuration
{
    public class FamilyTitleEntityTypeConfiguration : IEntityTypeConfiguration<FamilyTitle>
    {
        #region Constants

        /// <summary>
        /// Data fixa do seed: HasData precisa ser determinístico, senão cada
        /// geração de migration produziria uma alteração falsa
        /// </summary>
        private static readonly DateTime SEED_DATE = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region Members :: Configure()

        public void Configure(EntityTypeBuilder<FamilyTitle> builder)
        {
            builder.ToTable("FamilyTitles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.FamilyId)
                .IsRequired(false);

            // Título próprio morre junto com a família; título global não tem família
            builder.HasOne(x => x.Family)
                .WithMany(f => f.FamilyTitles)
                .HasForeignKey(x => x.FamilyId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            // Impede a mesma família cadastrar o mesmo título duas vezes.
            // Para os globais o Oracle não aplica a restrição, porque FamilyId é
            // nulo — por isso o serviço também valida o nome antes de inserir.
            builder.HasIndex(x => new { x.FamilyId, x.Name })
                .IsUnique();

            builder.HasData(
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000001"), Name = "Pai", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000002"), Name = "Mãe", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000003"), Name = "Cônjuge", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000004"), Name = "Filho", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000005"), Name = "Filha", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000006"), Name = "Irmão", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000007"), Name = "Irmã", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000008"), Name = "Avô", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000009"), Name = "Avó", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000010"), Name = "Neto", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000011"), Name = "Neta", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000012"), Name = "Tio", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000013"), Name = "Tia", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000014"), Name = "Sobrinho", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000015"), Name = "Sobrinha", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000016"), Name = "Primo", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000017"), Name = "Prima", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000018"), Name = "Padrasto", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000019"), Name = "Madrasta", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000020"), Name = "Enteado", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000021"), Name = "Enteada", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000022"), Name = "Sogro", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000023"), Name = "Sogra", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000024"), Name = "Genro", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000025"), Name = "Nora", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000026"), Name = "Cunhado", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000027"), Name = "Cunhada", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null },
                new { Id = Guid.Parse("a1b2c3d4-0000-4000-8000-000000000028"), Name = "Agregado", FamilyId = (Guid?)null, CreatedAt = SEED_DATE, LastUpdatedAt = (DateTime?)null }
            );
        }

        #endregion
    }
}
