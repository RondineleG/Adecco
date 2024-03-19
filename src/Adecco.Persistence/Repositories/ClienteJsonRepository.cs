using Adecco.Application.Dtos.Cliente;
using Adecco.Application.Extensions;
using Adecco.Core.Abstractions;
using Adecco.Core.Enums;

using System.Text;

namespace Adecco.Persistence.Repositories;

public sealed class ClienteJsonRepository : IClienteJsonRepository
{
    public ClienteJsonRepository()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\\Adecco.Persistence\\Data\\Json", "clientes.json");
        VerificarDiretorioEArquivo();
    }

    private readonly string _filePath;

    public async Task<List<Cliente>> ObterTodos()
    {
        if (!File.Exists(_filePath)) return new List<Cliente>();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        try
        {
            var jsonData = await File.ReadAllTextAsync(_filePath, Encoding.UTF8);
            var clientesResponseDtos = JsonSerializer.Deserialize<List<dynamic>>(jsonData, options);
            if (string.IsNullOrWhiteSpace(jsonData)) return new List<Cliente>();
            var clientes = JsonSerializer.Deserialize<List<ClienteResponseDto>>(jsonData, options);
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
        if (cliente == null || string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.CPF))
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
        var jsonData = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
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
            var jsonData = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, jsonData);
        }
    }

    public async Task<ContatoResponse> AtualizarContato(int clienteId, Contato contato)
    {
        var clientes = await ObterTodos();

        var cliente = await BuscarClientePodId(clienteId);
        if (cliente == null) throw new KeyNotFoundException("Cliente não encontrado");

        var contatoIndex = cliente.Contatos.FindIndex(c => c.Id == contato.Id);
        if (contatoIndex == -1) throw new KeyNotFoundException("Contato não encontrado");

        cliente.Contatos[contatoIndex] = contato;
        await SalvarDados(clientes);

        return new ContatoResponse(contato);
    }

    public async Task<EnderecoResponse> AtualizarEndereco(int clienteId, Endereco endereco)
    {
        var clientes = await ObterTodos();
        var cliente = await BuscarClientePodId(clienteId);
        if (cliente == null) throw new KeyNotFoundException("Cliente não encontrado");

        var enderecoIndex = cliente.Enderecos.FindIndex(e => e.Id == endereco.Id);
        if (enderecoIndex == -1) throw new KeyNotFoundException("Endereço não encontrado");

        cliente.Enderecos[enderecoIndex] = endereco;
        await SalvarDados(clientes);

        return new EnderecoResponse(endereco);
    }

    public async Task<ContatoResponse> IncluirContato(int clienteId, Contato contato)
    {
        var clientes = await ObterTodos();
        var cliente = clientes.FirstOrDefault(c => c.Id == clienteId);

        if (cliente == null) throw new KeyNotFoundException("Cliente não encontrado");

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

        if (cliente == null) throw new KeyNotFoundException("Cliente não encontrado");

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
        if (cliente == null) return;

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
        if (cliente == null) return;

        var endereco = cliente.Enderecos.FirstOrDefault(e => e.Id == enderecoId);
        if (endereco != null)
        {
            cliente.Enderecos.Remove(endereco);
            await SalvarDados(clientes);
        }
    }

    private async Task SalvarDados(List<Cliente> clientes)
    {
        var jsonData = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, jsonData);
    }

    private void VerificarDiretorioEArquivo()
    {
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

    private List<Cliente> ConverterParaClientes(List<dynamic> clientesResponseDto)
    {
        var clientes = new List<Cliente>();

        foreach (JsonElement clienteDto in clientesResponseDto)
        {
            var cliente = new Cliente();
            cliente.AdicionarId(clienteDto.GetProperty("Id").GetInt32());
            cliente.AtualizarCliente(
                clienteDto.GetProperty("Id").GetInt32(),
                clienteDto.GetProperty("Nome").GetString(),
                clienteDto.GetProperty("Email").GetString(),
                clienteDto.GetProperty("CPF").GetString(),
                clienteDto.GetProperty("RG").GetString());

            foreach (var contatoDto in clienteDto.GetProperty("Contatos").EnumerateArray())
            {
                var tipoContatoString = contatoDto.GetProperty("TipoContato").GetString();
                var tipoContato = EnumExtensions.ParseEnumFromDescription<ETipoContato>(tipoContatoString.Trim());

                var contato = new Contato(
                contatoDto.GetProperty("Id").GetInt32(),
                contatoDto.GetProperty("Nome").GetString(),
                contatoDto.GetProperty("DDD").GetInt32(),
                contatoDto.GetProperty("Telefone").GetInt64(),
                tipoContato);
                cliente.AdicionarContato(contato);
            }

            foreach (var enderecoDto in clienteDto.GetProperty("Enderecos").EnumerateArray())
            {
                var tipoEnderecoString = enderecoDto.GetProperty("TipoEndereco").GetString();
                var tipoEndereco = EnumExtensions.ParseEnumFromDescription<ETipoEndereco>(tipoEnderecoString.Trim());

                var endereco = new Endereco(
                enderecoDto.GetProperty("Id").GetInt32(),
                enderecoDto.GetProperty("Nome").GetString(),
                enderecoDto.GetProperty("CEP").GetString(),
                enderecoDto.GetProperty("Logradouro").GetString(),
                enderecoDto.GetProperty("Numero").GetString(),
                enderecoDto.GetProperty("Bairro").GetString(),
                enderecoDto.GetProperty("Complemento").GetString(),
                enderecoDto.GetProperty("Cidade").GetString(),
                enderecoDto.GetProperty("Estado").GetString(),
                enderecoDto.GetProperty("Referencia").GetString(),
                tipoEndereco);
                cliente.AdicionarEndereco(endereco);
            }

            clientes.Add(cliente);
        }

        return clientes;
    }
}