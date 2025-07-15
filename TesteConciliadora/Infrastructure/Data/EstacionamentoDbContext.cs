using Microsoft.EntityFrameworkCore;
using TesteConciliadora.Domain.Models;

namespace TesteConciliadora.Infrastructure.Data;


public class EstacionamentoDbContext : DbContext
{
    public EstacionamentoDbContext(DbContextOptions<EstacionamentoDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Mensalista> Mensalistas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
            .HasMany(c => c.Veiculos)
            .WithOne(v => v.Cliente)
            .HasForeignKey(v => v.ClienteId);

        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Mensalista)
            .WithOne(m => m.Cliente)
            .HasForeignKey<Mensalista>(m => m.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Veiculo>()
            .HasIndex(v => v.Placa)
            .IsUnique(); 
    }
}