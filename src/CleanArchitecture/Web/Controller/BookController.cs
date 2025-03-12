using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.Book.DTOs;
using CleanArchitecture.Shared.Models.Book.Requests;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class BookController : BaseController
{
    private readonly IBookService _bookService;
    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// get a list of books
    /// </summary>
    /// <param name="SearchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Books retrieved successfully.", typeof(ApiResponse<Pagination<BookDTO>>))]
    public async Task<IActionResult> Get(
        [FromQuery] BookSearchRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request parameters.");
        }

        var books = await _bookService.Get(request);

        return Success(books, "Books retrieved successfully");
    }

    /// <summary>
    /// get a book by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Book details retrieved successfully.", typeof(ApiResponse<BookDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID.", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Book not found.", typeof(ApiResponse))]
    public async Task<IActionResult> Get(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format");

        var book = await _bookService.Get(id);
        if (book is null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Book not found");

        return Success(book, "Book retrieved successfully");
    }

    /// <summary>
    /// add a book
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [SwaggerResponse(201, "Book added successfully.", typeof(ApiResponse<BookDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(CreateBookRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data");

        var book = await _bookService.Add(request, token);
        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
    }


    /// <summary>
    /// update a book
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [SwaggerResponse(200, "Book updated successfully.", typeof(ApiResponse<BookDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Book not found.")]
    public async Task<IActionResult> Update(UpdateBookRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data.");
        }

        var book = await _bookService.Update(request, token);
        if (book == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Book not found");

        return Success(book, "Book updated successfully.");
    }

    /// <summary>
    /// delete a book by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Book deleted successfully.")]
    [SwaggerResponse(404, "Book not found.")]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format.");

        var result = await _bookService.Delete(id, token);
        if (!result)
            throw new UserFriendlyException(ErrorCode.Internal, "Internal Error Happend");

        return NoContent();
    }
}
