using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FitnessClubWeb.Models
{
    public class FitnessClubContext : IdentityDbContext<User>
    {
        #region Constructor
        public FitnessClubContext(DbContextOptions<FitnessClubContext> options) : base(options)
        { }
        #endregion
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Visiting> Visitings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>()
                .Property(e => e.FIO)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Visitings);

            modelBuilder.Entity<Subscription>()
                .HasMany(e => e.Clients);
        }
    }
}
