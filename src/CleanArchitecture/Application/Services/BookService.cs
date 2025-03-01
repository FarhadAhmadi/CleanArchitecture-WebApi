using AutoMapper;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Shared.Models;
using CleanArchitecture.Shared.Models.Book;

namespace CleanArchitecture.Application.Services;

public class BookService(IUnitOfWork unitOfWork, IMapper mapper) : IBookService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Pagination<BookDTO>> Get(SearchRequest request)
    {
        var books = await _unitOfWork.BookRepository.ToPagination<Book, BookDTO>(
            tableName: DatabaseTableNames.Book,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            orderByColumn: "Id",
            ascending: request.IsDescending,
            selector: x => new BookDTO
            {
                Id = x.Id,
                Price = x.Price,
                Title = x.Title,
                Description = x.Description,
            }
        );

        return _mapper.Map<Pagination<BookDTO>>(books);
    }

    public async Task<BookDTO> Get(string id)
    {

        var bookWithDetails = await _unitOfWork.BookRepository.GetBookWithDetails(id);

        var book = await _unitOfWork.BookRepository.GetByIdAsync<Book , BookDTO>(
            tableName: DatabaseTableNames.Book,
            id: id,
            selector: x => new BookDTO()
            {
                Id = x.Id,
                Price = x.Price,    
                Description = x.Description,
                Title = x.Title,
            });

        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO> Add(AddBookRequest request, CancellationToken token)
    {
        var book = _mapper.Map<Book>(request);
        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.BookRepository.AddAsync(book), token);
        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO> Update(UpdateBookRequest request, CancellationToken token)
    {
        if (await _unitOfWork.BookRepository.AnyAsync(x => x.Id != request.Id))
            throw new UserFriendlyException("Book not found", "Book not found");

        var book = _mapper.Map<Book>(request);
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Update(book), token);
        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO> Delete(string id, CancellationToken token)
    {

        var existBook = await _unitOfWork.BookRepository.FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new UserFriendlyException("Book not found", "Book not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Delete(existBook), token);
        return _mapper.Map<BookDTO>(existBook);
    }
}
