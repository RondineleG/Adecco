using Adecco.Application.Dtos.Cliente;
using Adecco.Application.Dtos.Contato;
using Adecco.Application.Dtos.Endereco;
using Adecco.Application.Extensions;
using Adecco.Core.Enums;

using System.Text;

namespace Adecco.Persistence.Extensions;


public static class JsonFileHelper
{
    private static readonly string JsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\\Adecco.Persistence\\Data\\Json", "clientes.json");
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    public static string ArquivoJson()
    {
        return JsonFilePath;
    }
    public static List<ClienteResponseDto> LerArquivoJson()
    {
        VerificarDiretorioEArquivo();
        if (!File.Exists(JsonFilePath)) return new List<ClienteResponseDto>();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };

        try
        {
            var jsonData = File.ReadAllText(JsonFilePath, Encoding.UTF8);
            var clientesResponseDtos = JsonSerializer.Deserialize<List<dynamic>>(jsonData, options);
            if (string.IsNullOrWhiteSpace(jsonData)) return [];
            var clientes = ConverterParaClientes(clientesResponseDtos);
            return clientes ?? [];
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Arquivo não encontrado: {e.Message}");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Erro ao deserializar: {e.Message}");
        }
        return [];
    }

    private static void VerificarDiretorioEArquivo()
    {
        var directory = Path.GetDirectoryName(JsonFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(JsonFilePath))
        {
            File.Create(JsonFilePath).Dispose();
        }
    }
    public static void WriteToJsonFile<T>(List<T> data)
    {
        var jsonString = JsonSerializer.Serialize(data);
        File.WriteAllText(JsonFilePath, jsonString);
    }

    public static void WriteToJsonFile(List<ClienteResponseDto> data, string JsonFilePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(data, options);
        File.WriteAllText(JsonFilePath, jsonString);
    }



    private static List<ClienteResponseDto> ConverterParaClientes(List<dynamic> clientesResponseDto)
    {
        var clientes = new List<Cliente>();

        foreach (JsonElement clienteDto in clientesResponseDto)
        {
            var cliente = new Cliente();
            cliente.AdicionarId(GetIntProperty(clienteDto, "Id"));
            cliente.AtualizarCliente(
                GetIntProperty(clienteDto, "Id"),
                GetStringProperty(clienteDto, "Nome"),
                GetStringProperty(clienteDto, "Email"),
                GetStringProperty(clienteDto, "CPF"),
                GetStringProperty(clienteDto, "RG"));

            foreach (var contatoDto in clienteDto.GetProperty("Contatos").EnumerateArray())
            {
                var contato = new Contato(
                    GetIntProperty(contatoDto, "Id"),
                    GetStringProperty(contatoDto, "Nome"),
                    GetIntProperty(contatoDto, "DDD"),
                    GetIntProperty(contatoDto, "Telefone"),        
                    GetEnumProperty<ETipoContato>(contatoDto, "TipoContato"));

                cliente.AdicionarContato(contato);
            }

            foreach (var enderecoDto in clienteDto.GetProperty("Enderecos").EnumerateArray())
            {
                var endereco = new Endereco(
                    GetIntProperty(enderecoDto, "Id"),
                    GetStringProperty(enderecoDto, "Nome"),
                    GetStringProperty(enderecoDto, "CEP"),
                    GetStringProperty(enderecoDto, "Logradouro"),
                    GetStringProperty(enderecoDto, "Numero"),
                    GetStringProperty(enderecoDto, "Bairro"),
                    GetStringProperty(enderecoDto, "Complemento"),
                    GetStringProperty(enderecoDto, "Cidade"),
                    GetStringProperty(enderecoDto, "Estado"),
                    GetStringProperty(enderecoDto, "Referencia"),
                    GetEnumProperty<ETipoEndereco>(enderecoDto, "TipoEndereco"));

                cliente.AdicionarEndereco(endereco);
            }

            clientes.Add(cliente);
        }
        var response = ConverterClienteParaResponseDto(clientes);
        return response;
    }

    private static int GetIntProperty(JsonElement element, string propertyName)
    {
        return element.GetProperty(propertyName).GetInt32();
    }

    private static string? GetStringProperty(JsonElement element, string propertyName)
    {
        var propertyElement = element.GetProperty(propertyName);
        return propertyElement.ValueKind switch
        {
            JsonValueKind.String => propertyElement.GetString(),
            JsonValueKind.Number => propertyElement.GetRawText(),
            _ => throw new InvalidOperationException($"Tipo inesperado para a propriedade '{propertyName}'. Esperava-se String ou Number, obteve-se {propertyElement.ValueKind}.")
        };
    }



    private static TEnum GetEnumProperty<TEnum>(JsonElement element, string propertyName) where TEnum : struct, Enum
    {
        var propertyValue = GetStringProperty(element, propertyName).Trim();
        return EnumExtensions.ParseEnumFromDescription<TEnum>(propertyValue);
    }


    private static List<ClienteResponseDto> ConverterClienteParaResponseDto(List<Cliente> clientes)
    {
        var listaClientesResponseDto = new List<ClienteResponseDto>();

        foreach (var cliente in clientes)
        {
            var clienteResponseDto = new ClienteResponseDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                CPF = cliente.CPF,
                RG = cliente.RG,
                Contatos = cliente.Contatos.Select(contato => new ContatoResponseDto
                {
                    Id = contato.Id,
                    Nome = contato.Nome,
                    DDD = contato.DDD,
                    Telefone = contato.Telefone,
                    TipoContato = contato.TipoContato.ToDescriptionString()
                }).ToList(),
                Enderecos = cliente.Enderecos.Select(endereco => new EnderecoResponseDto
                {
                    Id = endereco.Id,
                    CEP = endereco.CEP,
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Bairro = endereco.Bairro,
                    Complemento = endereco.Complemento,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado,
                    Referencia = endereco.Referencia,
                    TipoEndereco = endereco.TipoEndereco.ToDescriptionString()
                }).ToList()
            };
            listaClientesResponseDto.Add(clienteResponseDto);
        }
        return listaClientesResponseDto;
    }

}

