using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Shared.Models.Response;
public class ApiResponse<T>
{
    public bool Success { get; }
    public string Message { get; }
    public T? Data { get; }

    private ApiResponse(bool success, string message, T? data)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Success") =>
        new(true, message, data);

    public static ApiResponse<T> Fail(string message) =>
        new(false, message, default);
}
public class ApiResponse
{
    public bool Success { get; }
    public string Message { get; }

    private ApiResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public static ApiResponse Fail(string message) =>
        new(false, message);
}

