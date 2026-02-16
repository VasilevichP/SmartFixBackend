using System.Net;

namespace SmartFix.Domain.Exceptions;

public class HttpException(HttpStatusCode statusCode, string message) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}