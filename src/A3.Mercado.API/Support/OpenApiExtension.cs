using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace A3.Mercado.API.Support
{
    public static class OpenApiExtension
    {
        #region Constantes
        public static readonly string ProjectName = Assembly
            .GetEntryAssembly()?
            .GetName()
            .Name;

        public static readonly string DocumentationRoute = "api-docs";
        public static readonly string Version = "v1";
        #endregion

        /// <summary>
        /// Metodo que extiende para configurar el Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddLLOpenApi(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddControllers(options =>
            {
            })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            services.AddEndpointsApiExplorer();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Version,
                    new OpenApiInfo
                    {
                        Title = "A3 Mercado Api",
                        Description = "Api challenge ",
                        Version = Version,
                    });
                // form 2 to generate the swagger documentation
                foreach (var name in Directory.GetFiles(AppContext.BaseDirectory, "*.XML", SearchOption.TopDirectoryOnly))
                {
                    options.IncludeXmlComments(filePath: name);
                }
            });

            return services;
        }



        /// <summary>
        /// Metodo que extiende para la utilizacion del app
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseLLOpenApi(
            this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = $"{ProjectName} OpenApi UI";
                options.SwaggerEndpoint($"../swagger/{Version}/swagger.json", $"{ProjectName} {Version}");
                options.RoutePrefix = DocumentationRoute;
            });
            app.MapControllers();

            return app;
        }
    }
}
