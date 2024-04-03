namespace Adecco.Persistence.Contexts.Interfaces;
public interface IApplicattionDataContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Contato> Contatos { get; set; }

    public DbSet<Endereco> Enderecos { get; set; }
}
