namespace Adecco.Persistence.Repositories;

public sealed class ClienteJsonRepository : IClienteJsonRepository
{
    public ClienteJsonRepository()
    {
        _filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            @"..\\Adecco.Persistence\\Data\\Json",
            "clientes.json"
        );

        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Dispose();
        }
    }

    private readonly string _filePath;
    private List<Cliente> _clientes;

    public List<Cliente> ObterTodos()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        if (!File.Exists(_filePath))
        {
            return new List<Cliente>();
        }
        var jsonData = File.ReadAllText(_filePath);

        try
        {
            var clientes = JsonSerializer.Deserialize<List<Cliente>>(jsonData, options);
            return clientes;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Erro ao deserializar: {e.Message}");
            Console.WriteLine($"JSON: {jsonData}");
        }
        return new List<Cliente>();
    }

    public Cliente ObterPorId(int id)
    {
        var clientes = ObterTodos();
        return clientes.Find(c => c.Id == id);
    }

    public void Adicionar(Cliente cliente)
    {
        var clientes = ObterTodos();
        var novoId = clientes.Any() ? clientes.Max(c => c.Id) + 1 : 1;
        cliente.AdicionarId(novoId);
        clientes.Add(cliente);
        Salvar(clientes);
    }

    public void Atualizar(int id, Cliente clienteAtualizado)
    {
        var clientes = ObterTodos();
        var index = clientes.FindIndex(c => c.Id == id);
        if (index != -1)
        {
            clientes[index] = clienteAtualizado;
            Salvar(_clientes);
        }
    }

    public void Remover(int id)
    {
        var clientes = ObterTodos();
        var cliente = clientes.Find(c => c.Id == id);
        if (cliente != null)
        {
            clientes.Remove(cliente);
            Salvar(_clientes);
        }
    }

    public void AtualizarContato(int clienteId, Contato contato)
    {
        var cliente = _clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var contatoExistente = cliente.Contatos.FirstOrDefault(c => c.Id == contato.Id);
            if (contatoExistente != null)
            {
                cliente.Contatos.Remove(contatoExistente);
                cliente.Contatos.Add(contato);
            }
            else
            {
                cliente.Contatos.Add(contato);
            }
            Salvar(_clientes);
        }
    }

    public void AtualizarEndereco(int clienteId, Endereco endereco)
    {
        var clientes = ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var enderecoExistente = cliente.Enderecos.FirstOrDefault(e => e.Id == endereco.Id);
            if (enderecoExistente != null)
            {
                cliente.Enderecos.Remove(enderecoExistente);
            }
            cliente.Enderecos.Add(endereco);
            Salvar(_clientes);
        }
    }

    public void RemoverContato(int clienteId, int contatoId)
    {
        var clientes = ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var contatoParaRemover = cliente.Contatos.FirstOrDefault(c => c.Id == contatoId);
            if (contatoParaRemover != null)
            {
                cliente.Contatos.Remove(contatoParaRemover);
                Salvar(clientes);
            }
        }
    }

    public void RemoverEndereco(int clienteId, int enderecoId)
    {
        var clientes = ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var enderecoParaRemover = cliente.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
            if (enderecoParaRemover != null)
            {
                cliente.Enderecos.Remove(enderecoParaRemover);
                Salvar(clientes);
            }
        }
    }

    public void IncluirContato(int clienteId, Contato contato)
    {
        var clientes = ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var novoId = cliente.Contatos.Any() ? cliente.Contatos.Max(c => c.Id) + 1 : 1;
            contato.AdicionarId(novoId);
            cliente.Contatos.Add(contato);
            Salvar(clientes);
        }
        else
        {
            throw new ArgumentException($"Cliente com ID {clienteId} não encontrado.");
        }
    }

    public void IncluirEndereco(int clienteId, Endereco endereco)
    {
        var clientes = ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente != null)
        {
            var novoId = cliente.Enderecos.Any() ? cliente.Enderecos.Max(e => e.Id) + 1 : 1;
            endereco.AdicionarId(novoId);
            cliente.Enderecos.Add(endereco);
            Salvar(clientes);
        }
        else
        {
            throw new ArgumentException($"Cliente com ID {clienteId} não encontrado.");
        }
    }

    private void Salvar(List<Cliente> clientes)
    {
        var jsonData = JsonSerializer.Serialize(
            clientes,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(_filePath, jsonData);
    }
}
