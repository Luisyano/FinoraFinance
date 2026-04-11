using FinoraFinance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinoraFinance.Data
{
    public class AppDbContext : IdentityDbContext<Usuario>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}
        public DbSet<Cuenta> Cuentas { get; set; }
        public DbSet<Etiqueta> Etiquetas { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Cuenta)
                .WithMany()
                .HasForeignKey(t => t.CuentaId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.Etiqueta)
                .WithMany()
                .HasForeignKey(t => t.EtiquetaId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Transaccion>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
