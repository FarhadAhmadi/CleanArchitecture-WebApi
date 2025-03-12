using System.Linq.Expressions;
using AutoMapper;
using Azure.Core;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Constants;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    public BookService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<Pagination<BookDTO>> Get(BookSearchRequest request)
    {
        var books = await _unitOfWork.BookRepository.ToPagination<BookDTO>(
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            filter: null,
            include: query => query.Include(b => b.Author).Include(b => b.Publisher),
            orderBy: b => b.Id,
            ascending: true,
            selector: x => new BookDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                Author = new AuthorDTO
                {
                    Id = x.Author.Id,
                    Bio = x.Author.Bio,
                    Name = x.Author.Name,
                },
                Publisher = new PublisherDTO
                {
                    Id = x.Publisher.Id,
                    Name = x.Publisher.Name,
                },
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return books;
    }

    public async Task<BookDTO> Get(string id)
    {
        var book = await _unitOfWork.BookRepository.GetSingleData<BookDTO>(
            filter: p => p.Id == id,   // Filter by Id
            include: query => query.Include(b => b.Author).Include(b => b.Publisher),
            orderBy: p => p.Id,  // Order by Name, if needed
            ascending: true,  // Ascending order
            selector: x => new BookDTO
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                Author = new AuthorDTO
                {
                    Id = x.Author.Id,
                    Bio = x.Author.Bio,
                    Name = x.Author.Name,
                },
                Publisher = new PublisherDTO
                {
                    Id = x.Publisher.Id,
                    Name = x.Publisher.Name,
                },
                CreatedOn = x.CreatedOn,
                CreatorId = x.CreatorId,
                UpdatedOn = x.UpdatedOn,
                UpdaterId = x.UpdaterId,
            });

        return book;
    }

    public async Task<BookDTO> Add(CreateBookRequest request, CancellationToken token)
    {
        var book = _mapper.Map<Book>(request);
        book.CreatedOn = DateTime.UtcNow;
        book.CreatorId = _currentUser.GetCurrentUserId();

        // Check if Author exists
        var isAuthorExist = await _unitOfWork.AuthorRepository.AnyAsyncWithDapper<Author>(DatabaseTableNames.Author, request.AuthorId);
        if (!isAuthorExist)
            throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        // Check if Publisher exists
        var isPublisherExist = await _unitOfWork.PublisherRepository.AnyAsyncWithDapper<Publisher>(DatabaseTableNames.Publisher, request.PublisherId);
        if (!isPublisherExist)
            throw new UserFriendlyException(ErrorCode.NotFound, "Publisher not found");

        await _unitOfWork.ExecuteTransactionAsync(async () => await _unitOfWork.BookRepository.AddAsync(book), token);

        return _mapper.Map<BookDTO>(book);
    }

    public async Task<BookDTO> Update(UpdateBookRequest request, CancellationToken token)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Book, request.Id);

        if (book == null)
            throw new UserFriendlyException(ErrorCode.NotFound , "Book not found");

        // Check if Author exists
        var isAuthorExist = await _unitOfWork.AuthorRepository.AnyAsyncWithDapper<Author>(DatabaseTableNames.Author, request.AuthorId);
        if (!isAuthorExist)
            throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        // Check if Publisher exists
        var isPublisherExist = await _unitOfWork.PublisherRepository.AnyAsyncWithDapper<Publisher>(DatabaseTableNames.Publisher, request.PublisherId);
        if (!isPublisherExist)
            throw new UserFriendlyException(ErrorCode.NotFound, "Publisher not found");

        // Only update the fields that are provided in the request
        if (!string.IsNullOrEmpty(request.Title)) book.Title = request.Title;
        if (!string.IsNullOrEmpty(request.Description)) book.Description = request.Description;
        if (request.Price.HasValue) book.Price = request.Price.Value;

        // Update timestamp and user information
        book.UpdatedOn = DateTimeOffset.UtcNow;
        book.UpdaterId = _currentUser.GetCurrentUserId();
        book.AuthorId = request.AuthorId;
        book.PublisherId = request.PublisherId;

        // Perform the update within a transaction
        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Update(book), token);

        // Return the updated book as DTO
        return _mapper.Map<BookDTO>(book);
    }

    public async Task<bool> Delete(string id, CancellationToken token)
    {
        var book = await _unitOfWork.BookRepository.GetByIdAsyncWithDapper(DatabaseTableNames.Book, id)
            ?? throw new UserFriendlyException(ErrorCode.NotFound, "Book not found");

        await _unitOfWork.ExecuteTransactionAsync(() => _unitOfWork.BookRepository.Delete(book), token);
        return true;
    }
}
