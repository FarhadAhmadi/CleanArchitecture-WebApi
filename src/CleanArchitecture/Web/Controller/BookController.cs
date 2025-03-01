using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models;
using CleanArchitecture.Shared.Models.Book;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class BookController(IBookService bookService, ILogger<BookController> logger) : BaseController
{
    private readonly IBookService _bookService = bookService;
    private readonly ILogger<BookController> _logger = logger;

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
            return Failure("Invalid ID format");

        var book = await _bookService.Get(id);
        if (book is null)
            return NotFoundResponse("Book not found");

        return Success(book, "Book retrieved successfully");
    }

    /// <summary>
    /// get a list of books
    /// </summary>
    /// <param name="SearchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Books retrieved successfully.", typeof(ApiResponse<Pagination<BookDTO>>))]
    public async Task<IActionResult> Get(
        [FromQuery] SearchRequest request)
    {
        var books = await _bookService.Get(request);
        return Success(books, "Books retrieved successfully");
    }

    /// <summary>
    /// add a book
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse(201, "Book added successfully.", typeof(ApiResponse<BookDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(AddBookRequest request, CancellationToken token)
        => Ok(await _bookService.Add(request, token));

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
        => Ok(await _bookService.Update(request, token));

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
        => Ok(await _bookService.Delete(id, token));
}
