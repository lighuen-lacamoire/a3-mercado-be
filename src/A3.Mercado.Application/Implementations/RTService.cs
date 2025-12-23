using A3.Mercado.Domain.DTOs;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace A3.Mercado.Application.Implementations
{
    public class RTService : BackgroundService
    {
        private readonly Channel<List<InstrumentPriceDto>> _channel = Channel.CreateUnbounded<List<InstrumentPriceDto>>();
        private int _counter = 0;

        public RTService(Channel<List<InstrumentPriceDto>> channel) => _channel = channel;

        public IAsyncEnumerable<List<InstrumentPriceDto>> GetUpdates(

            CancellationToken ct) =>
            _channel.Reader.ReadAllAsync(ct);


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var process = Process.GetCurrentProcess();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken); // every 2s

                var symbols = new[] { "AL30", "AL31", "GL1", "AMZN" };
                
                List<InstrumentPriceDto> list = new List<InstrumentPriceDto>();


                foreach (var symbol in symbols)
                {
                    //var symbol = symbols[Random.Shared.Next(symbols.Length)];
                    var variation = Math.Round((decimal)(-5 + Random.Shared.NextDouble() * 10), 2);
                    var accumulatedVolume = Math.Round((decimal)(1000 + Random.Shared.NextDouble() * 5000), 2);
                    // Pick a random symbol and price
                    var price = Math.Round((decimal)(100 + Random.Shared.NextDouble() * 50), 2);

                    var id = DateTime.UtcNow.ToString("o");

                    var update = new InstrumentPriceDto { Code = symbol, Price = price, Variation = variation, AccumulatedVolume = accumulatedVolume };
                    list.Add(update);
                }

                await _channel.Writer.WriteAsync(list, stoppingToken);
            }
        }
    }
}
