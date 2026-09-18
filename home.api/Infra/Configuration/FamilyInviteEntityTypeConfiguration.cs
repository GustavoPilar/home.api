using home.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace home.api.Infra.Configuration
{
    public class FamilyInviteEntityTypeConfiguration : IEntityTypeConfiguration<FamilyInvite>
    {
        public void Configure(EntityTypeBuilder<FamilyInvite> builder)
        {
            builder.ToTable("FamilyInvites");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.TargetEmail)
                .IsRequired(false)
                .HasMaxLength(256);

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.UseCount)
                .IsRequired();

            builder.Property(x => x.CreatedByUserId)
                .IsRequired();

            // O aceite busca pelo hash: o índice único é o caminho da consulta
            // e ao mesmo tempo impede colisão de token
            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            builder.HasIndex(x => x.FamilyId);

            builder.HasOne(x => x.Family)
                .WithMany(f => f.FamilyInvites)
                .HasForeignKey(x => x.FamilyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
