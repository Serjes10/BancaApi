using Microsoft.EntityFrameworkCore;
using BancaApi.Domain.Entities;

namespace BancaApi.Data;


public class BancaDbContext : DbContext
{
    public BancaDbContext(DbContextOptions<BancaDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }
    public DbSet<Transaccion> Transacciones { get; set; }

    public DbSet<ContadorCuentas> ContadorCuentas {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuenta>().HasIndex(c => c.numeroCuenta).IsUnique();
        base.OnModelCreating(modelBuilder);

    }
}
