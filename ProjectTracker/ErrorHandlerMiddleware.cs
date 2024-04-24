using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjectTracker;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            HttpResponse response = context.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await response.WriteAsync(JsonSerializer.Serialize(new { message = e.Message }));
        }
    }
}