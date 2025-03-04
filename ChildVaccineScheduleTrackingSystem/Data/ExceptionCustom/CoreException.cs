using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Data.ExceptionCustom;

// Base exception for the application
public class CoreException : Exception
{
    public string Code { get; }

    public int StatusCode { get; set; }

    public Dictionary<string, object>? AdditionalData { get; set; }

    public CoreException(string errorCode, string message, int statusCode = StatusCodes.Status500InternalServerError)
        : base(message) // Ensure base message is set
    {
        Code = errorCode;
        StatusCode = statusCode;
    }
}

// General error exception with structured error details
public class ErrorException : Exception
{
    public int StatusCode { get; }

    public ErrorDetail ErrorDetail { get; }

    public ErrorException(int statusCode, string errorCode, string message)
        : base(message) // Ensure base message is set
    {
        StatusCode = statusCode;
        ErrorDetail = new ErrorDetail
        {
            ErrorCode = errorCode,
            ErrorMessage = message
        };
    }

    public ErrorException(int statusCode, ErrorDetail errorDetail)
        : base(errorDetail.ErrorMessage?.ToString()) // Ensure base message is set
    {
        StatusCode = statusCode;
        ErrorDetail = errorDetail;
    }
}

// Specific bad request exception
public class BadRequestException : ErrorException
{
    public BadRequestException(string errorCode, string message)
        : base(StatusCodes.Status400BadRequest, errorCode, message)
    {
    }

    public BadRequestException(ICollection<KeyValuePair<string, ICollection<string>>> errors)
        : base(StatusCodes.Status400BadRequest, new ErrorDetail
        {
            ErrorCode = "bad_request",
            ErrorMessage = errors
        })
    {
    }
}

// Error detail class for structured responses
public class ErrorDetail
{
    [JsonPropertyName("errorCode")]
    public string? ErrorCode { get; set; }

    [JsonPropertyName("errorMessage")]
    public object? ErrorMessage { get; set; }
}
