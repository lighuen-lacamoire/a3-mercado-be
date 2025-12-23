using A3.Mercado.Domain.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace A3.Mercado.Application.Support.Handlers
{
    public class InstrumentPricesManager
    {
        private readonly ConcurrentDictionary<string, List<WebSocket>> _subscribers = new();
        private readonly object _lock = new();
        public ILogger<InstrumentPricesManager> LoggerHandler { get; }

        public InstrumentPricesManager(ILogger<InstrumentPricesManager> logger)
        {
            LoggerHandler = logger;
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

        public async Task HandleConnectionAsync(WebSocket socket, CancellationToken ct)
        {
            var buffer = new byte[1024 * 4];

            while (!ct.IsCancellationRequested && socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, ct);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var request = JsonSerializer.Deserialize<WebSocketRequestBodyDto<List<InstrumentRowDto>>>(message);

                    if (request.Payload.Any())
                    {
                        foreach (var instrument in request.Payload)
                        {
                            lock (_lock)
                            {
                                if (!_subscribers.ContainsKey(instrument.Code))
                                {
                                    _subscribers.TryAdd(instrument.Code, new List<WebSocket>());
                                }
                                _subscribers[instrument.Code].Add(socket);
                            }
                        }
                        continue;
                    }

                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                }
            }
        }
        /*
        public async Task BroadcastUpdateAsync(StockUpdate update)
        {
            if (_subscribers.TryGetValue(update.Symbol, out var sockets))
            {
                var json = JsonSerializer.Serialize(update);
                var bytes = Encoding.UTF8.GetBytes(json);

                foreach (var socket in sockets.ToArray())
                {
                    if (socket.State == WebSocketState.Open)
                    {
                        await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }
        */
    }
}
