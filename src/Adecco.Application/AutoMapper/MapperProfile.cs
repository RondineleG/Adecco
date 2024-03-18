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

        CreateMap<Cliente, ClienteResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF))
            .ForMember(dest => dest.RG, opt => opt.MapFrom(src => src.RG))
            .ForMember(dest => dest.Contatos, opt => opt.MapFrom(src => src.Contatos))
            .ForMember(dest => dest.Enderecos, opt => opt.MapFrom(src => src.Enderecos));

        CreateMap<ClienteRequestDto, Cliente>();
    }
}