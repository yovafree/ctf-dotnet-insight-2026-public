using Microsoft.EntityFrameworkCore;

namespace CardamomoPay.Api.Domain;

public class CardamomoDbContext(DbContextOptions<CardamomoDbContext> options) : DbContext(options)
{
    public DbSet<Socio> Socios => Set<Socio>();
    public DbSet<Cuenta> Cuentas => Set<Cuenta>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Socio>().HasIndex(s => s.Correo).IsUnique();
        b.Entity<Cuenta>().HasIndex(c => c.Numero).IsUnique();
        b.Entity<Cuenta>().Property(c => c.Saldo).HasColumnType("decimal(18,2)");
        b.Entity<Movimiento>().Property(m => m.Monto).HasColumnType("decimal(18,2)");
    }
}
