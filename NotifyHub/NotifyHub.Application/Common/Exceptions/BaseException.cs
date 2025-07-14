using System.Net;

namespace NotifyHub.Application.Common.Exceptions;

public class BaseException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public BaseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    
    public BaseException(string message) { }
}