using AutoMapper;
using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Author.Requests;
using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Category.DTOs;
using CleanArchitecture.Shared.Models.Category.Requests;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Publisher.Requests;
using CleanArchitecture.Shared.Models.Response;
using CleanArchitecture.Shared.Models.User;

namespace CleanArchitecture.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Book, BookDTO>().ReverseMap();
        CreateMap<Book, BookEditDTO>().ReverseMap();
        CreateMap<Book, CreateBookRequest>().ReverseMap();
        CreateMap<Book, UpdateBookRequest>().ReverseMap();

        CreateMap<Author, AuthorDTO>().ReverseMap();
        CreateMap<Author, AuthorEditDTO>().ReverseMap();
        CreateMap<Author, CreateAuthorRequest>().ReverseMap();
        CreateMap<Author, UpdateAuthorRequest>().ReverseMap();

        CreateMap<Publisher, PublisherDTO>().ReverseMap();
        CreateMap<Publisher, PublisherEditDTO>().ReverseMap();
        CreateMap<Publisher, CreatePublisherRequest>().ReverseMap();
        CreateMap<Publisher, UpdatePublisherRequest>().ReverseMap();

        CreateMap<Category, CategoryDTO>().ReverseMap();
        CreateMap<Category, CategoryEditDTO>().ReverseMap();
        CreateMap<Category, CreateCategoryRequest>().ReverseMap();
        CreateMap<Category, UpdateCategoryRequest>().ReverseMap();

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
