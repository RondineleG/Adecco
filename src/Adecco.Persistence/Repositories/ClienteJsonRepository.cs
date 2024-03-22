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
        VerificarDiretorioEArquivo(_filePath);
    }

    private readonly string _filePath;

    public async Task<List<Cliente>> ObterTodos()
    {
        if (!File.Exists(_filePath))
            return new List<Cliente>();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        try
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(jsonData))
                return new List<Cliente>();

            var clientesResponseDtos =
                JsonSerializer.Deserialize<List<ClienteResponseDto>>(jsonData, options)
                ?? new List<ClienteResponseDto>();

            var clientesResponseDto = ConverterParaClientes(clientesResponseDtos);
            return clientesResponseDto ?? new List<Cliente>();
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Arquivo não encontrado: {e.Message}");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Erro ao deserializar: {e.Message}");
        }

        return new List<Cliente>();
    }

    public async Task<IEnumerable<Cliente>> ListarClientes(string? nome, string? email, string? cpf)
    {
        var clientes = await ObterTodos();
        if (!string.IsNullOrEmpty(nome))
        {
            clientes = clientes.Where(c => c.Nome.Contains(nome)).ToList();
        }
        if (!string.IsNullOrEmpty(email))
        {
            clientes = clientes.Where(c => c.Email.Contains(email)).ToList();
        }
        if (!string.IsNullOrEmpty(cpf))
        {
            clientes = clientes.Where(c => c.CPF.Contains(cpf)).ToList();
        }

        return await Task.FromResult<IEnumerable<Cliente>>(clientes);
    }

    public async Task<Cliente> BuscarClientePodId(int clienteId)
    {
        var cliente = await BuscarClientePodId(clienteId);
        if (cliente == null)
        {
            throw new KeyNotFoundException("Cliente não encontrado");
        }
        return cliente;
    }

    public async Task<ClienteResponse> AdicionarCliente(Cliente cliente)
    {
        if (
            cliente == null
            || string.IsNullOrEmpty(cliente.Nome)
            || string.IsNullOrEmpty(cliente.CPF)
        )
        {
            throw new ArgumentException("Dados do cliente inválidos.");
        }
        var clientes = await ObterTodos();
        var novoId = clientes.Any() ? clientes.Max(c => c.Id) + 1 : 1;
        cliente.AdicionarId(novoId);

        clientes.Add(cliente);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var jsonData = JsonSerializer.Serialize(clientes, options);
        await File.WriteAllTextAsync(_filePath, jsonData, Encoding.UTF8);
        return new ClienteResponse(cliente);
    }

    public async Task<ClienteResponse> AtualizarCliente(int clienteId, Cliente clienteAtualizado)
    {
        var clientes = await ObterTodos();
        var cliente = await BuscarClientePodId(clienteId);
        if (clienteId == -1)
        {
            throw new KeyNotFoundException("Cliente não encontrado");
        }

        clientes[clienteId] = clienteAtualizado;
        var jsonData = JsonSerializer.Serialize(
            clientes,
            new JsonSerializerOptions { WriteIndented = true }
        );
        await File.WriteAllTextAsync(_filePath, jsonData);
        return new ClienteResponse(clienteAtualizado);
    }

    public async void RemoverCliente(int clienteId)
    {
        var clientes = await ObterTodos();
        var cliente = await BuscarClientePodId(clienteId);
        if (cliente != null)
        {
            clientes.Remove(cliente);
            var jsonData = JsonSerializer.Serialize(
                clientes,
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(_filePath, jsonData);
        }
    }

    public async Task<ContatoResponse> AtualizarContato(int clienteId, Contato contato)
    {
        var clientes = await ObterTodos();

        var cliente = await BuscarClientePodId(clienteId);
        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        var contatoIndex = cliente.Contatos.FindIndex(c => c.Id == contato.Id);
        if (contatoIndex == -1)
            throw new KeyNotFoundException("Contato não encontrado");

        cliente.Contatos[contatoIndex] = contato;
        await SalvarDados(clientes);

        return new ContatoResponse(contato);
    }

    public async Task<EnderecoResponse> AtualizarEndereco(int clienteId, Endereco endereco)
    {
        var clientes = await ObterTodos();
        var cliente = await BuscarClientePodId(clienteId);
        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        var enderecoIndex = cliente.Enderecos.FindIndex(e => e.Id == endereco.Id);
        if (enderecoIndex == -1)
            throw new KeyNotFoundException("Endereço não encontrado");

        cliente.Enderecos[enderecoIndex] = endereco;
        await SalvarDados(clientes);

        return new EnderecoResponse(endereco);
    }

    public async Task<ContatoResponse> IncluirContato(int clienteId, Contato contato)
    {
        var clientes = await ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);

        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        var novoContatoId = cliente.Contatos.Any() ? cliente.Contatos.Max(c => c.Id) + 1 : 1;
        contato.AdicionarId(novoContatoId);

        cliente.Contatos.Add(contato);
        await SalvarDados(clientes);

        return new ContatoResponse(contato);
    }

    public async Task<EnderecoResponse> IncluirEndereco(int clienteId, Endereco endereco)
    {
        var clientes = await ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);

        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        var novoEnderecoId = cliente.Enderecos.Any() ? cliente.Enderecos.Max(e => e.Id) + 1 : 1;
        endereco.AdicionarId(novoEnderecoId);

        cliente.Enderecos.Add(endereco);
        await SalvarDados(clientes);

        return new EnderecoResponse(endereco);
    }

    public async void RemoverContato(int clienteId, int contatoId)
    {
        var clientes = await ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente == null)
            return;

        var contato = cliente.Contatos.FirstOrDefault(c => c.Id == contatoId);
        if (contato != null)
        {
            cliente.Contatos.Remove(contato);
            await SalvarDados(clientes);
        }
    }

    public async void RemoverEndereco(int clienteId, int enderecoId)
    {
        var clientes = await ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente == null)
            return;

        var endereco = cliente.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
        if (endereco != null)
        {
            cliente.Enderecos.Remove(endereco);
            await SalvarDados(clientes);
        }
    }

    private async Task SalvarDados(List<Cliente> clientes)
    {
        var jsonData = JsonSerializer.Serialize(
            clientes,
            new JsonSerializerOptions { WriteIndented = true }
        );
        await File.WriteAllTextAsync(_filePath, jsonData);
    }

    private static void VerificarDiretorioEArquivo(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(filePath))
        {
            using var fileStream = File.Create(filePath);
        }
    }

    private List<Cliente> ConverterParaClientes(List<ClienteResponseDto> clientesResponseDto)
    {
        var clientes = new List<Cliente>();

        foreach (var clienteDto in clientesResponseDto)
        {
            var cliente = new Cliente();

            var id = clienteDto.Id;
            var nome = clienteDto.Nome ?? string.Empty;
            var email = clienteDto.Email ?? string.Empty;
            var cpf = clienteDto.CPF ?? string.Empty;
            var rg = clienteDto.RG ?? string.Empty;

            cliente.AdicionarId(id);
            cliente.AtualizarCliente(id, nome, email, cpf, rg);

            foreach (var contatoDto in clienteDto.Contatos)
            {
                var tipoContato = EnumExtensions.ParseEnumFromDescription<ETipoContato>(
                    contatoDto.TipoContato ?? string.Empty
                );
                var contato = new Contato(
                    contatoDto.Id,
                    contatoDto.Nome ?? string.Empty,
                    contatoDto.DDD,
                    contatoDto.Telefone,
                    tipoContato
                );
                cliente.AdicionarContato(contato);
            }

            foreach (var enderecoDto in clienteDto.Enderecos)
            {
                var tipoEndereco = EnumExtensions.ParseEnumFromDescription<ETipoEndereco>(
                    enderecoDto.TipoEndereco ?? string.Empty
                );
                var endereco = new Endereco(
                    enderecoDto.Id,
                    enderecoDto.Nome ?? string.Empty,
                    enderecoDto.CEP ?? string.Empty,
                    enderecoDto.Logradouro ?? string.Empty,
                    enderecoDto.Numero ?? string.Empty,
                    enderecoDto.Bairro ?? string.Empty,
                    enderecoDto.Complemento ?? string.Empty,
                    enderecoDto.Cidade ?? string.Empty,
                    enderecoDto.Estado ?? string.Empty,
                    enderecoDto.Referencia ?? string.Empty,
                    tipoEndereco
                );
                cliente.AdicionarEndereco(endereco);
            }

            clientes.Add(cliente);
        }

        return clientes;
    }
}
