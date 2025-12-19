using A3.Mercado.API.Support;
using A3.Mercado.Application.Support;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddLLOpenApi(builder.Configuration);
builder.Services.AddLLServices();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAccess_To_API",
        policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    ));
var app = builder.Build();

app.UseCors(builder => builder
   .AllowAnyOrigin()
   .AllowAnyMethod()
   .AllowAnyHeader());
app.UseLLWebSocket();
app.UseLLOpenApi();

app.UseAuthorization();

app.UseStaticFiles();

app.Run();
