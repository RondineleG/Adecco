namespace Adecco.Persistence.Contexts;

public sealed class ApplicattionDataContext(DbContextOptions<ApplicattionDataContext> options)
    : DbContext(options)
{
    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Contato> Contatos { get; set; }

    public DbSet<Endereco> Enderecos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Contato>().ToTable("Contatos");
        modelBuilder.Entity<Contato>().HasKey(p => p.Id);
        modelBuilder.Entity<Contato>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<Contato>().Property(p => p.Nome).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Contato>().Property(p => p.DDD).IsRequired().IsFixedLength().HasMaxLength(2);
        modelBuilder.Entity<Contato>().Property(p => p.Telefone).IsRequired().HasMaxLength(9);
        modelBuilder.Entity<Contato>().Property(p => p.TipoContato).IsRequired();
        modelBuilder.Entity<Contato>().Property(p => p.ClienteId).IsRequired(false);
        modelBuilder.Entity<Contato>().HasOne(c => c.Cliente).WithMany(cl => cl.Contatos)
            .HasForeignKey(c => c.ClienteId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Endereco>().ToTable("Enderecos");
        modelBuilder.Entity<Endereco>().HasKey(p => p.Id);
        modelBuilder.Entity<Endereco>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<Endereco>().Property(p => p.Nome).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Endereco>().Property(p => p.CEP).IsRequired().IsFixedLength().HasMaxLength(8);
        modelBuilder.Entity<Endereco>().Property(p => p.Logradouro).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Endereco>().Property(p => p.Numero).IsRequired().HasMaxLength(20);
        modelBuilder.Entity<Endereco>().Property(p => p.Bairro).IsRequired().HasMaxLength(30);
        modelBuilder.Entity<Endereco>().Property(p => p.Complemento).IsRequired(false).HasMaxLength(30);
        modelBuilder.Entity<Endereco>().Property(p => p.Cidade).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Endereco>().Property(p => p.Estado).IsRequired().HasMaxLength(2);
        modelBuilder.Entity<Endereco>().Property(p => p.Referencia).IsRequired(false).HasMaxLength(50);
        modelBuilder.Entity<Endereco>().Property(p => p.TipoEndereco).IsRequired();
        modelBuilder.Entity<Endereco>().Property(p => p.ClienteId).IsRequired(false);
        modelBuilder.Entity<Endereco>().HasOne(e => e.Cliente).WithMany(cl => cl.Enderecos)
            .HasForeignKey(e => e.ClienteId).IsRequired(false).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Cliente>().ToTable("Clientes");
        modelBuilder.Entity<Cliente>().HasKey(p => p.Id);
        modelBuilder.Entity<Cliente>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<Cliente>().Property(p => p.Nome).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Cliente>().Property(p => p.Email).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Cliente>().Property(p => p.CPF).IsRequired().IsFixedLength().HasMaxLength(11);
        modelBuilder.Entity<Cliente>().Property(p => p.RG).IsRequired().IsFixedLength().HasMaxLength(11);
        modelBuilder.Entity<Cliente>().Property(p => p.ContatoId).IsRequired();
        modelBuilder.Entity<Cliente>().Property(p => p.EnderecoId).IsRequired();
    }
}