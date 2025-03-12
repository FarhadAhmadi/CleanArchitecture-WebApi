using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Shared.Models.Author.DTOs;
using CleanArchitecture.Shared.Models.Author.Requests;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class AuthorController : BaseController
{
    private readonly IAuthorService _authorService;
    private readonly ILogger<AuthorController> _logger;
    public AuthorController(IAuthorService authorService, ILogger<AuthorController> logger)
    {
        _authorService = authorService;
        _logger = logger;
    }
    /// <summary>
    /// get a list of Authors
    /// </summary>
    /// <param name="SearchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Authors retrieved successfully.", typeof(ApiResponse<Pagination<AuthorDTO>>))]
    public async Task<IActionResult> Get(
        [FromQuery] AuthorSearchRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request parameters.");
        }

        var authors = await _authorService.Get(request);

        return Success(authors, "Authors retrieved successfully");
    }

    /// <summary>
    /// get a author by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Author details retrieved successfully.", typeof(ApiResponse<AuthorDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID.", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Author not found.", typeof(ApiResponse))]
    public async Task<IActionResult> Get(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format");

        var author = await _authorService.Get(id);
        if (author is null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        return Success(author, "Author retrieved successfully");
    }

    /// <summary>
    /// add a author
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [SwaggerResponse(201, "Author added successfully.", typeof(ApiResponse<AuthorDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(CreateAuthorRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data");

        var author = await _authorService.Add(request, token);
        return CreatedAtAction(nameof(Get), new { id = author.Id }, author);
    }


    /// <summary>
    /// update a author
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [SwaggerResponse(200, "Author updated successfully.", typeof(ApiResponse<AuthorDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Author not found.")]
    public async Task<IActionResult> Update(UpdateAuthorRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data.");
        }

        var author = await _authorService.Update(request, token);
        if (author == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Author not found");

        return Success(author, "Author updated successfully.");
    }

    /// <summary>
    /// delete a author by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Author deleted successfully.")]
    [SwaggerResponse(404, "Author not found.")]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format.");

        var result = await _authorService.Delete(id, token);
        if (!result)
            throw new UserFriendlyException(ErrorCode.Internal, "Internal Error Happend");

        return NoContent();
    }

}
