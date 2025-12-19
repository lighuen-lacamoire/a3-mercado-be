using A3.Mercado.Application.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3.Mercado.Application.Support
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddLLServices(
            this IServiceCollection services)
        {
            services.AddSingleton<InstrumentPricesWebSocket>();

            return services;
        }
    }
}
