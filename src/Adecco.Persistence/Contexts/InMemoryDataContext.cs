using Adecco.Persistence.Contexts.Interfaces;

using System.Reflection;

namespace Adecco.Persistence.Contexts;

public class InMemoryDataContext(DbContextOptions<InMemoryDataContext> options) : DbContext(options),IApplicattionDataContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Contato> Contatos { get; set; }

    public DbSet<Endereco> Enderecos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}