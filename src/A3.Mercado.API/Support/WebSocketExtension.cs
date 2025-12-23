using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace A3.Mercado.API.Support
{
    public static class WebSocketExtension
    {


        public static WebApplication UseLLWebSocket(
            this WebApplication app)
        {
            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromMinutes(1)
            };

            app.UseWebSockets(webSocketOptions);

            return app;
        }
    }
}
