using AutoMapper;
using CleanArchitecture.Shared.Models;
using CleanArchitecture.Shared.Models.Book;
using CleanArchitecture.Shared.Models.User;

namespace CleanArchitecture.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookDTO>().ReverseMap();
        CreateMap<Book, AddBookRequest>().ReverseMap();
        CreateMap<Book, UpdateBookRequest>().ReverseMap();

        CreateMap<User, UserSignInRequest>().ReverseMap();
        CreateMap<User, UserSignInResponse>().ReverseMap();
        CreateMap<User, UserSignUpRequest>().ReverseMap();
        CreateMap<User, UserSignUpResponse>().ReverseMap();
        CreateMap<User, UserProfileResponse>().ReverseMap();

        CreateMap(typeof(Pagination<>), typeof(Pagination<>)).ConvertUsing(typeof(PaginationConverter<,>));
    }
}

public class PaginationConverter<TSource, TDestination> : ITypeConverter<Pagination<TSource>, Pagination<TDestination>>
{
    private readonly IMapper _mapper;

    public PaginationConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public Pagination<TDestination> Convert(Pagination<TSource> source, Pagination<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = _mapper.Map<List<TDestination>>(source.Items);
        return new Pagination<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
    }
}
