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
            .ForMember(
                src => src.TipoContato,
                opt => opt.MapFrom(src => (ETipoContato)src.TipoContato)
            );

        CreateMap<Endereco, EnderecoResponseDto>()
            .ForMember(
                src => src.TipoEndereco,
                opt => opt.MapFrom(src => src.TipoEndereco.ToDescriptionString())
            );

        CreateMap<EnderecoRequestDto, Endereco>()
            .ForMember(
                src => src.TipoEndereco,
                opt => opt.MapFrom(src => (ETipoEndereco)src.TipoEndereco)
            );

        CreateMap<Cliente, ClienteResponseDto>();
        CreateMap<ClienteRequestDto, Cliente>();
    }
}
