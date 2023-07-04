namespace LexisApi.Infrastructure.Middlewares.CustomExceptionMiddleware;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder builder) => builder.UseMiddleware<ExceptionMiddleware>();
}