using A3.Mercado.Domain.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace A3.Mercado.Application.Implementations
{
    public class InstrumentPricesWebSocket
    {
        public ILogger<InstrumentPricesWebSocket> LoggerHandler { get; }

        public InstrumentPricesWebSocket(ILogger<InstrumentPricesWebSocket> logger)
        {
            LoggerHandler = logger;
        }
        public async Task Echo(WebSocket webSocket)
        {
            var receiveBuffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                /**/

                var receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
                LoggerHandler.LogInformation("WebSocket Recibido: {message}", receivedMessage);

                List<InstrumentPriceDto> prices = new List<InstrumentPriceDto>
                    {
                        new InstrumentPriceDto
                        {
                            Code = "ABC",
                            Price = 123.45M,
                            Variation = 1.23M,
                            AccumulatedVolume = 1000M
                        },
                        new InstrumentPriceDto
                        {
                            Code = "DEF",
                            Price = 67.89M,
                            Variation = -0.56M,
                            AccumulatedVolume = 2000M
                        }
                    };

                string message = JsonSerializer.Serialize(prices);
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                LoggerHandler.LogInformation("WebSocket Enviado: {message}", message);
                //var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);
                /**/
                /*
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);
                */
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
        public async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var receiveBuffer = new ArraySegment<byte>(new byte[4096]);
            while (webSocket.State == WebSocketState.Open)
            {
                var receiveResult = await webSocket.ReceiveAsync(receiveBuffer, CancellationToken.None);

                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    var receivedMessage = Encoding.UTF8.GetString(receiveBuffer.Array, 0, receiveResult.Count);
                    LoggerHandler.LogInformation("WebSocket Recibido: {message}", receivedMessage);
                    try
                    {

                        var list = JsonSerializer.Deserialize<WebSocketRequestBodyDto<List<InstrumentRowDto>>>(receivedMessage, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        List<InstrumentPriceDto> prices = new List<InstrumentPriceDto>
                    {
                        new InstrumentPriceDto
                        {
                            Code = "AL30",
                            Price = 123.45M,
                            Variation = 1.23M,
                            AccumulatedVolume = 1000M
                        },
                        new InstrumentPriceDto
                        {
                            Code = "AL35",
                            Price = 67.89M,
                            Variation = -0.56M,
                            AccumulatedVolume = 2000M
                        }
                    };

                        string message = JsonSerializer.Serialize(prices);
                        byte[] buffer = Encoding.UTF8.GetBytes(message);
                        LoggerHandler.LogInformation("WebSocket Enviado: {message}", message);
                        //var arraySegment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        LoggerHandler.LogError(ex, ex.Message);
                    }
                }
                else if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    LoggerHandler.LogInformation("WebSocket Cerrado: {desc}", webSocket.CloseStatus.Value);
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

                }
            }
        }
    }
}
