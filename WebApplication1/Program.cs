using Microsoft.OpenApi.Models;
using System.Net.WebSockets;
using WebApplication1.Auth;
using WebApplication1.Helpers;
using WebApplication1.Hub;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.IgnoreObsoleteActions();
            c.IgnoreObsoleteProperties();
            c.CustomSchemaIds(type => type.FullName);
            c.AddSecurityDefinition("ApiKey",new OpenApiSecurityScheme
            {
                Description="The API Key to access APIs",
                Type=SecuritySchemeType.ApiKey,
                Name= "X-API-Key",
                In=ParameterLocation.Header,
                Scheme="ApiKeyScheme"
            });
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                Type=ReferenceType.SecurityScheme,
                Id="ApiKey"
                },
                In=ParameterLocation.Header
            };
            var requirement = new OpenApiSecurityRequirement
            {
                {scheme,new List<string>() }
            };
            c.AddSecurityRequirement(requirement);
        });
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
                              });
        });
        // We have to add WebSocketHub as Singleton because it needs to live until program closes
        builder.Services.AddSingleton(typeof(WebSocketHub), new WebSocketHub());
        // register validation 
        builder.Services.AddTransient<IApiKeyValidation, ApiKeyValidation>();

        builder.Services.AddScoped<CheckHelper>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //register api key middleware 
        app.UseMiddleware<ApiKeyMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.UseCors(MyAllowSpecificOrigins);
        // we have to add this. If you do not context.WebSockets.IsWebSocketRequest is always false
        app.UseWebSockets(new WebSocketOptions
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120), // you cna set ping-pong time period in here
            ReceiveBufferSize = 4 * 1024 // you can specify buffer size here (default is 4kb)
        });
        #region WebSocket
        // We need WebSocketHub for socket operations so we get it with GetService
        WebSocketHub _webSocketHub = (WebSocketHub)app.Services.GetService(typeof(WebSocketHub));

        // If a request does not match any of the endpoints it will drop here 
        app.Use(async (context, next) =>
        {
            try
            {
                // You can check header and request in here. For example
                // if(context.Response.Headers...)
                // if(context.Request.Query...)

                // We just check IsWebSocketRequest
                if (context.WebSockets.IsWebSocketRequest)
                {
                    // We accept the socket connection
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                    // we use underscore to discard return here because we do not have to waite return
                    _webSocketHub.AddSocket(webSocket);

                    // We have to hold the context here if we release it, server will close it
                    while (webSocket.State == WebSocketState.Open)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(1));
                    }

                    // if socket status is not open ,remove it
                    _webSocketHub.RemoveSocket(webSocket);

                    // check socket state if it is not closed, close it
                    if (webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection End", CancellationToken.None);
                    }
                }
                else
                {
                    await next();
                }
            }
            catch (Exception exp)
            {
                //log ws connection error
            }
        });

        #endregion

        app.Run();
    }
}
