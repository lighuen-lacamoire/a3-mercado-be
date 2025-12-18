using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace A3.Mercado.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        #region Dependencias
        private readonly ILogger<InstrumentsController> _logger;
        #endregion

        #region Constructor
        public InstrumentsController(
            ILogger<InstrumentsController> logger
            )
        {
            _logger = logger;
        }
        #endregion

        #region Endpoints
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }
        #endregion
    }
}
