using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.Category.DTOs;
using CleanArchitecture.Shared.Models.Category.Requests;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// get a list of categorirs
    /// </summary>
    /// <param name="SearchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Categorys retrieved successfully.", typeof(ApiResponse<Pagination<CategoryDTO>>))]
    public async Task<IActionResult> Get(
        [FromQuery] CategorySearchRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request parameters.");
        }

        var categorirs = await _categoryService.Get(request);

        return Success(categorirs, "Categorys retrieved successfully");
    }

    /// <summary>
    /// get a category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Category details retrieved successfully.", typeof(ApiResponse<CategoryDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID.", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Category not found.", typeof(ApiResponse))]
    public async Task<IActionResult> Get(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format");

        var category = await _categoryService.Get(id);
        if (category is null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Category not found");

        return Success(category, "Category retrieved successfully");
    }

    /// <summary>
    /// add a category
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [SwaggerResponse(201, "Category added successfully.", typeof(ApiResponse<CategoryDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(CreateCategoryRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data");

        var category = await _categoryService.Add(request, token);
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }


    /// <summary>
    /// update a category
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [SwaggerResponse(200, "Category updated successfully.", typeof(ApiResponse<CategoryDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Category not found.")]
    public async Task<IActionResult> Update(UpdateCategoryRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data.");
        }

        var category = await _categoryService.Update(request, token);
        if (category == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Category not found");

        return Success(category, "Category updated successfully.");
    }

    /// <summary>
    /// delete a category by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Category deleted successfully.")]
    [SwaggerResponse(404, "Category not found.")]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format.");

        var result = await _categoryService.Delete(id, token);
        if (!result)
            throw new UserFriendlyException(ErrorCode.Internal, "Internal Error Happend");

        return NoContent();
    }
}
