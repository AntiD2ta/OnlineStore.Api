using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OnlineStore.Api.Data.Models;

namespace OnlineStore.Api.Data
{
    public class OnlineStoreContext : IdentityDbContext<Usuario>
    {
        public OnlineStoreContext(DbContextOptions<OnlineStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Orden> Ordenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)



        }
    }
}