using home.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace home.api.Infra.Configuration
{
    public class HomeEntityTypeConfiguration : IEntityTypeConfiguration<Home>
    {
        public void Configure(EntityTypeBuilder<Home> builder)
        {
            builder.ToTable("Homes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Address)
                .IsRequired(false)
                .HasMaxLength(150);

            builder.Property(x => x.AddressNumber)
                .IsRequired(false);

            builder.Property(x => x.ZipCode)
                .IsRequired(false)
                .HasMaxLength(8);

            builder.HasOne(x => x.User)
                .WithMany(u => u.Homes)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // SetNull explícito: sem isso o EF usa ClientSetNull e só desfaz o vínculo
            // se os lares estiverem carregados, quebrando com erro de FK no banco
            builder.HasOne(x => x.Family)
                .WithMany(f => f.Homes)
                .HasForeignKey(x => x.FamilyId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
