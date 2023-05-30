using System.Net;
using System.Runtime.CompilerServices;
using MassTransit;

namespace WDA.Shared;

[Serializable]
public class HttpException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public Guid TraceId { get; set; }

    public HttpException(string? message) : base(message)
    {
        TraceId = NewId.NextGuid();
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public HttpException(string? message, Exception? innerException) : base(message, innerException)
    {
        TraceId = NewId.NextGuid();
        StatusCode = HttpStatusCode.InternalServerError;
    }

    public HttpException(string message, HttpStatusCode statusCode) : base(message)
    {
        TraceId = NewId.NextGuid();
        StatusCode = statusCode;
    }

    public static void ThrowIfNull(object? param, HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        [CallerArgumentExpression("param")] string paramName = "")
    {
        if (param == null)
        {
            throw new HttpException($"Handled Exception. {paramName} is null", statusCode);
        }
    }
}