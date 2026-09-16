using home.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace home.api.Infra.Configuration
{
    public class HomeEntityTypeConfiguration : IEntityTypeConfiguration<Home>
    {
        public void Configure(EntityTypeBuilder<Home> builder)
        {
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
                .IsRequired();
        }
    }
}
