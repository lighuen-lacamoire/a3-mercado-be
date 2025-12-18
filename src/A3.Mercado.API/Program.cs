using A3.Mercado.API.Support;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddLLOpenApi(builder.Configuration);

var app = builder.Build();

app.UseLLOpenApi();

app.UseAuthorization();

app.UseStaticFiles();

app.Run();
