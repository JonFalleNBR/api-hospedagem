using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Hospedagem.Models;

namespace API.Hospedagem.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hospede> Hospedes { get; set; } = null!;
        public virtual DbSet<Quarto> Quartos { get; set; } = null!;
        public virtual DbSet<Reserva> Reservas { get; set; } = null!;

        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<Funcionario> Funcionarios { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=NOTRP\\SQLEXPRESS;Database=HotelDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospede>(entity =>
            {
                entity.ToTable("Hospede");

                entity.HasIndex(e => e.Cpf, "UQ__Hospede__C1F8973133CEFA5D")
                    .IsUnique();

                entity.Property(e => e.Cpf)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CPF");

                entity.Property(e => e.DataCadastro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Telefone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Quarto>(entity =>
            {
                entity.ToTable("Quarto");

                entity.HasIndex(e => e.Numero, "UQ__Quarto__7E532BC6DDAE882A")
                    .IsUnique();

                entity.Property(e => e.PrecoPorNoite).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.ToTable("Reserva");

                entity.Property(e => e.DataCheckin).HasColumnType("datetime");

                entity.Property(e => e.DataCheckout).HasColumnType("datetime");

                entity.Property(e => e.StatusReserva)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ValorTotal).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Hospede)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.HospedeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Hospede");

                entity.HasOne(d => d.Quarto)
                    .WithMany(p => p.Reservas)
                    .HasForeignKey(d => d.QuartoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reserva_Quarto");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
