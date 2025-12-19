using A3.Mercado.Application.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace A3.Mercado.API.Controllers
{
    [Route("ws")]
    [ApiController]
    public class WebSocketController : ControllerBase
    {
        #region Dependencias
        private readonly InstrumentPricesWebSocket _instrumentPricesSocket;
        private readonly ILogger<WebSocketController> _logger;
        #endregion

        #region Constructor
        public WebSocketController(
            InstrumentPricesWebSocket socketService,
            ILogger<WebSocketController> logger
            )
        {
            _instrumentPricesSocket = socketService;
            _logger = logger;
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
