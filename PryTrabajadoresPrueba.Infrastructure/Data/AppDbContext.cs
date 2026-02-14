using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PryTrabajadoresPrueba.Domain.Entities;

namespace PryTrabajadoresPrueba.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Trabajador> Trabajadores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=TrabajadoresPrueba;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trabajador>(entity =>
        {
            entity.HasKey(e => e.IdTrb).HasName("PK__Trabajad__2BC59D77B70FF774");

            entity.HasIndex(e => e.NumeroDocumento, "UQ__Trabajad__A42025882A0FF3B1").IsUnique();

            entity.Property(e => e.IdTrb)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Direccion).HasMaxLength(200);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoUrl).HasMaxLength(255);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(20);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.TipoDocumento).HasMaxLength(20);
        });
        modelBuilder.HasSequence("SeqTrabajadores");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
