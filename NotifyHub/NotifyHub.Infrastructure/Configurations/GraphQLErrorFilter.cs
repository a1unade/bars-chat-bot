using Microsoft.Extensions.Logging;
using NotifyHub.Application.Common.Exceptions;

namespace NotifyHub.Infrastructure.Configurations;

public class GraphQLErrorFilter: IErrorFilter
{
    private readonly ILogger<GraphQLErrorFilter> _logger;

    public GraphQLErrorFilter(ILogger<GraphQLErrorFilter> logger)
    {
        _logger = logger;
    }

    public IError OnError(IError error)
    {
        _logger.LogError(error.Exception, "GraphQL Error: {Message}", error.Message);
        
        if (error.Extensions != null
            && error.Extensions.TryGetValue("code", out var codeObj)
            && codeObj is string codeStr
            && codeStr.Contains("Validator", StringComparison.OrdinalIgnoreCase))
        {
            return error
                .WithMessage(error.Message)
                .WithCode("VALIDATION_ERROR")
                .SetExtension("validationCode", codeStr);
        }
        
        if (error.Exception is ArgumentException argEx)
        {
            return error
                .WithMessage(argEx.Message)
                .WithCode("ARGUMENT_ERROR");
        }

        if (error.Exception is NotFoundException nfEx)
        {
            return error
                .WithMessage(nfEx.Message)
                .WithCode("NOT_FOUND");
        }
        
        return error
            .WithMessage("Что-то пошло не так при выполнении запроса.")
            .WithCode("UNEXPECTED");
    }
}
