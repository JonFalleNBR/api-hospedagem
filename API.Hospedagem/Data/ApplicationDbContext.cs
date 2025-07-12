using Microsoft.EntityFrameworkCore;         // <- obrigatório
using API.Hospedagem.Models;                 // <- seus models

namespace API.Hospedagem.Data
{
    // Precisa herdar de DbContext
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hospede> Hospedes { get; set; }
        public DbSet<Quarto> Quartos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hospede>().ToTable("Hospede");

            // idem para Quarto e Reserva
            modelBuilder.Entity<Quarto>().ToTable("Quarto");
            modelBuilder.Entity<Reserva>().ToTable("Reserva");
        }
    }
}
