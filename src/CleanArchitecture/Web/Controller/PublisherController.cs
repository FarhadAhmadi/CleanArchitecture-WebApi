using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.Publisher.DTOs;
using CleanArchitecture.Shared.Models.Publisher.Requests;
using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class PublisherController : BaseController
{
    private readonly IPublisherService _publisherService;
    private readonly ILogger<PublisherController> _logger;

    public PublisherController(IPublisherService publisherService, ILogger<PublisherController> logger)
    {
        _publisherService = publisherService;
        _logger = logger;
    }

    /// <summary>
    /// get a list of publishers
    /// </summary>
    /// <param name="SearchRequest"></param>
    /// <returns></returns>
    [HttpGet]
    [SwaggerResponse(200, "Publishers retrieved successfully.", typeof(ApiResponse<Pagination<PublisherDTO>>))]
    public async Task<IActionResult> Get(
        [FromQuery] PublisherSearchRequest request)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request parameters.");
        }

        var publishers = await _publisherService.Get(request);

        return Success(publishers, "Publishers retrieved successfully");
    }

    /// <summary>
    /// get a publisher by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Publisher details retrieved successfully.", typeof(ApiResponse<PublisherDTO>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID.", typeof(ApiResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Publisher not found.", typeof(ApiResponse))]
    public async Task<IActionResult> Get(string id)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format");

        var publisher = await _publisherService.Get(id);
        if (publisher is null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Publisher not found");

        return Success(publisher, "Publisher retrieved successfully");
    }

    /// <summary>
    /// add a publisher
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    [SwaggerResponse(201, "Publisher added successfully.", typeof(ApiResponse<PublisherDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> Add(CreatePublisherRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data");

        var publisher = await _publisherService.Add(request, token);
        return CreatedAtAction(nameof(Get), new { id = publisher.Id }, publisher);
    }


    /// <summary>
    /// update a publisher
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    [SwaggerResponse(200, "Publisher updated successfully.", typeof(ApiResponse<PublisherDTO>))]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(404, "Publisher not found.")]
    public async Task<IActionResult> Update(UpdatePublisherRequest request, CancellationToken token)
    {
        if (!ModelState.IsValid)
        {
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid request data.");
        }

        var publisher = await _publisherService.Update(request, token);
        if (publisher == null)
            throw new UserFriendlyException(ErrorCode.NotFound, "Publisher not found");

        return Success(publisher, "publisher updated successfully.");
    }

    /// <summary>
    /// delete a publisher by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{id}")]
    [SwaggerResponse(200, "Publisher deleted successfully.")]
    [SwaggerResponse(404, "Publisher not found.")]
    public async Task<IActionResult> Delete(string id, CancellationToken token)
    {
        if (!Guid.TryParse(id, out _))
            throw new UserFriendlyException(ErrorCode.BadRequest, "Invalid ID format.");

        var result = await _publisherService.Delete(id, token);
        if (!result)
            throw new UserFriendlyException(ErrorCode.Internal, "Internal Error Happend");

        return NoContent();
    }
}
