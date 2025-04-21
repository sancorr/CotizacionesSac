using CotizacionesDomain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CotizacionesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //[controller] es un token dinamico que se reemplaza por el nombre de la clase sin "Controller"
    public class PriceStreamController : ControllerBase
    {
        private readonly IPriceStreamService _webSocketService;

        public PriceStreamController(IPriceStreamService repoWebSocket) 
        {
            _webSocketService = repoWebSocket;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Suscribe([FromBody] List<string> tickers)
        {
            try
            {
                await _webSocketService.SubscribeAsync(tickers);
                return Ok("Subscripcion enviada correctamente");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
