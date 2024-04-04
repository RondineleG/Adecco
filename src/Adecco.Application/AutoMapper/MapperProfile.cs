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
        CreateMap<CustomResult<Cliente>, CustomResult<ClienteResponseDto>>();
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


public static class CustomResultMapperExtensions
{
    public static CustomResult<TDestination> MapCustomResult<TSource, TDestination>(
        this CustomResult<TSource> source,
        IMapper mapper)
    {
        if (source == null)
            return CustomResult<TDestination>.WithNoContent();

        if (source.Status == CustomResultStatus.Success && source.Data != null)
        {
            var destinationData = mapper.Map<TDestination>(source.Data);
            return CustomResult<TDestination>.Success(destinationData);
        }

        if (source.Status == CustomResultStatus.HasError && source.Error != null)
            return CustomResult<TDestination>.WithError(source.Error.Description);

        if (source.Status == CustomResultStatus.HasValidation && source.Validations != null)
            return CustomResult<TDestination>.WithValidations(source.Validations.ToArray());

        if (source.Status == CustomResultStatus.EntityNotFound && source.EntityWarning != null)
            return CustomResult<TDestination>.EntityNotFound(
                source.EntityWarning.Name,
                source.EntityWarning.Id,
                source.EntityWarning.Message);

        if (source.Status == CustomResultStatus.EntityAlreadyExists && source.EntityWarning != null)
            return CustomResult<TDestination>.EntityAlreadyExists(
                source.EntityWarning.Name,
                source.EntityWarning.Id,
                source.EntityWarning.Message);

        return CustomResult<TDestination>.WithNoContent();
    }
}