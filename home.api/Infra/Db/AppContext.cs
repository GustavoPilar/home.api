using home.api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace home.api.Infra.Db
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
    {
        #region Properties

        public DbSet<Home> Homes { get; set; } = default!;

        public DbSet<Family> Families { get; set; } = default!;

        public DbSet<UserFamily> UserFamilies { get; set; } = default!;

        public DbSet<FamilyTitle> FamilyTitles { get; set; } = default!;

        public DbSet<FamilyInvite> FamilyInvites { get; set; } = default!;

        #endregion

        #region Members :: OnModelCreating()

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        #endregion
    }
}
