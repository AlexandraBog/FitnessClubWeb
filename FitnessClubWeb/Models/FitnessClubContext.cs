using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FitnessClubWeb.Models
{
    /// <summary>
    /// класс, который общается в бд
    /// </summary>
    public class FitnessClubContext : IdentityDbContext<User>
    {
        public FitnessClubContext(DbContextOptions<FitnessClubContext> options) : base(options)
        { }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Visiting> Visitings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // использование Fluent API

            modelBuilder.Entity<Client>()
                .Property(e => e.FIO)   // в FIO клинета не должно быть юникодовских символов
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Visitings); // установка связи один ко многим

            modelBuilder.Entity<Subscription>()
                .HasMany(e => e.Clients);     
        }
    }
}
