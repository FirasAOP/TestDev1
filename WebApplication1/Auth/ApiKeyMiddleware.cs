using System.Net;

namespace WebApplication1.Auth
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiKeyValidation _apiKeyValidation;
        public ApiKeyMiddleware(RequestDelegate next, IApiKeyValidation apiKeyValidation)
        {
            _next = next;
            _apiKeyValidation = apiKeyValidation;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(context.Request.Headers[Constants.ApiKeyHeaderName]))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("API Key missing!");
                return;
            }
            string? userApiKey = context.Request.Headers[Constants.ApiKeyHeaderName];
            if (!_apiKeyValidation.IsValidApiKey(userApiKey!))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key!");
                return;
            }
            await _next(context);
        }
    }
}
