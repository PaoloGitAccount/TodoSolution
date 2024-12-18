namespace WebAPI.Middlewares;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class MyMiddleware
{
    private readonly RequestDelegate next;

    public MyMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Do something before the next middleware in the pipeline
        await next(context); 
        // Do something after the next middleware
    }
}

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
        await _next(context);
    }
}
