namespace NotifyHub.WebApi.Configurations;

public static class CorsPolicyConfiguration
{
    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder builder)
    {
        return builder.UseCors(b => b
            .WithOrigins("http://localhost:5000")
            .AllowAnyMethod()
            .AllowAnyHeader());
    }
}