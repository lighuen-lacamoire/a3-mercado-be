using A3.Mercado.Application.Implementations;
using A3.Mercado.Application.Support.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;

namespace A3.Mercado.API.Controllers
{
    [Route("ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        #region Dependencias
        private readonly InstrumentPricesManager _instrumentPricesSocket;
        private readonly ILogger<WebSocketController> _logger;
        private readonly RTService _rtService;
        #endregion

        #region Constructor
        public WebSocketController(
            InstrumentPricesManager socketService,
            ILogger<WebSocketController> logger,
            RTService rtService
            )
        {
            _instrumentPricesSocket = socketService;
            _logger = logger;
            _rtService = rtService;
        }
        #endregion

        #region Endpoints
        [HttpGet("prices")]
        public async Task Get()
        {
            
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                await _instrumentPricesSocket.HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            
        }
        #endregion
    }
}
