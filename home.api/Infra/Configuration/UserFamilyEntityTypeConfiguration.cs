using home.api.Domain.Entities;
using home.api.Domain.Enums;
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

            // Gravado como texto para que a coluna seja legível direto no banco
            builder.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasConversion<string>()
                .HasDefaultValue(MembershipRole.Member);

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

            // Apagar um título em uso apenas desatribui o membro, nunca bloqueia
            builder.HasOne(x => x.FamilyTitle)
                .WithMany(t => t.UserFamilies)
                .HasForeignKey(x => x.FamilyTitleId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Impede o mesmo usuário ser associado duas vezes à mesma família
            builder.HasIndex(x => new { x.UserId, x.FamilyId })
                .IsUnique();

            builder.HasIndex(x => x.FamilyTitleId);
        }
    }
}
