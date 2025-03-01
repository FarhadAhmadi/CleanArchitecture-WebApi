using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller;

[ApiController]
[Route("api/[controller]/")]
public class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string message = "Success")
        => Ok(ApiResponse<T>.SuccessResponse(data, message));

    protected IActionResult CreatedResponse<T>(string actionName, object routeValues, T data, string message = "Resource created successfully")
        => CreatedAtAction(actionName, routeValues, ApiResponse<T>.SuccessResponse(data, message));

    protected IActionResult NoContentResponse(string message = "No content available")
        => NoContent();

    protected IActionResult Failure(string message, int statusCode = StatusCodes.Status400BadRequest)
        => StatusCode(statusCode, ApiResponse.Fail(message));

    protected IActionResult ForbiddenResponse(string message = "You are not authorized to perform this action")
        => StatusCode(StatusCodes.Status403Forbidden, ApiResponse.Fail(message));

    protected IActionResult NotFoundResponse(string message = "Resource not found")
        => StatusCode(StatusCodes.Status404NotFound, ApiResponse.Fail(message));
}
