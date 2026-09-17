using home.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace home.api.Infra.Configuration
{
    public class UserFamilyEntityTypeConfiguration : IEntityTypeConfiguration<UserFamily>
    {
        public void Configure(EntityTypeBuilder<UserFamily> builder)
        {
            builder.ToTable("UserFamilies");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(u => u.UserFamilies)
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Family)
                .WithMany(f => f.UserFamilies)
                .HasForeignKey(x => x.FamilyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Impede o mesmo usuário ser associado duas vezes à mesma família
            builder.HasIndex(x => new { x.UserId, x.FamilyId })
                .IsUnique();
        }
    }
}
