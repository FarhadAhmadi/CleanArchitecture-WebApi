using CleanArchitecture.Shared.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Controller;

[ApiController]
[Route("api/[controller]/")]
public class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string message = "Success")
        => Ok(ApiResponse<T>.SuccessResponse(data, message));
}
