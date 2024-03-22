namespace Adecco.Application.AutoMapper;

public sealed class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Contato, ContatoResponseDto>()
            .ForMember(
                src => src.TipoContato,
                opt => opt.MapFrom(src => src.TipoContato.ToDescriptionString())
            );

        CreateMap<ContatoRequestDto, Contato>()
            .ConstructUsing(src => new Contato())
            .ForMember(
                dest => dest.TipoContato,
                opt => opt.MapFrom(src => ConvertToETipoContato(src.TipoContato))
            );

        CreateMap<Endereco, EnderecoResponseDto>()
            .ForMember(
                src => src.TipoEndereco,
                opt => opt.MapFrom(src => src.TipoEndereco.ToDescriptionString())
            );

        CreateMap<EnderecoRequestDto, Endereco>()
            .ConstructUsing(src => new Endereco())
            .ForMember(
                dest => dest.TipoEndereco,
                opt => opt.MapFrom(src => ConvertToETipoEndereco(src.TipoEndereco))
            );

        CreateMap<ContatoRequestDto, ContatoResponseDto>();
        CreateMap<EnderecoRequestDto, EnderecoResponseDto>();

        CreateMap<Cliente, ClienteResponseDto>();
        CreateMap<ClienteRequestDto, Cliente>();
    }

    private static ETipoContato ConvertToETipoContato(int tipoContatoValue)
    {
        var byteValue = Convert.ToByte(tipoContatoValue);
        if (!Enum.IsDefined(typeof(ETipoContato), byteValue))
            throw new ArgumentOutOfRangeException(
                "TipoContato",
                "Valor inválido para o tipo de contato"
            );
        return (ETipoContato)byteValue;
    }

    private static ETipoEndereco ConvertToETipoEndereco(int tipoEnderecoValue)
    {
        var byteValue = Convert.ToByte(tipoEnderecoValue);
        if (!Enum.IsDefined(typeof(ETipoEndereco), byteValue))
            throw new ArgumentOutOfRangeException(
                "TipoContato",
                "Valor inválido para o tipo de contato"
            );
        return (ETipoEndereco)byteValue;
    }
}
