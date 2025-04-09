using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotizacionesDomain.Interfaces;

namespace CotizacionesApi.Controllers
{
	[ApiController]
	[Route("api/markets")]
	public class CotizacionesController : ControllerBase
	{
		private readonly ICotizacionesRepository _CotizacionesRepo;

		public CotizacionesController(ICotizacionesRepository repo)
		{
			_CotizacionesRepo = repo;
		}

		[HttpGet("stocks-codes")]
		public async Task<IActionResult> GetStocksCodes()
		{
			try
			{
				var stocksCodes = await _CotizacionesRepo.GetStocksMarketsCode();

				if (stocksCodes == null)
					return NotFound();

				if (!stocksCodes.Any())
					return Ok("No hay elementos que mostrar");

				return Ok(stocksCodes);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("indexes-codes")]
		public async Task<IActionResult> GetIndexesCodes()
		{
			try
			{
				var indexesCodes = await _CotizacionesRepo.GetIndexesMarketsCode();

				if (indexesCodes == null)
					return NotFound();

				if (!indexesCodes.Any())
					return Ok("No hay elementos que mostrar");

				return Ok(indexesCodes);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("all-indexes")]
		public async Task<IActionResult> GetAllIndexes()
		{
			try
			{
				var allIndexes = await _CotizacionesRepo.GetAllIndexes();

				if (allIndexes == null)
					return NotFound();
				if (!allIndexes.Any())
					return Ok("No hay elementos que mostrar");

				return Ok(allIndexes);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("usa-indexes")]
		public async Task<IActionResult> GetUsaIndexes()
		{
			try
			{
				var usaIndexes = await _CotizacionesRepo.GetUsaIndexes();
				if (usaIndexes == null)
					return NotFound();
				if (!usaIndexes.Any())
					return Ok("No hay elementos que mostrar");

				return Ok(usaIndexes);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("indexes-by-exchange")]
		public async Task<IActionResult> GetIndexesByExchange([FromQuery] string exchangeCode)
		{
			try
			{
				var indexesByExchange = await _CotizacionesRepo.GetIndexesMarketCountry(exchangeCode);
				if (indexesByExchange == null)
					return NotFound();
				if (!indexesByExchange.Any())
					return Ok("No hay elementos que mostrar");

				return Ok(indexesByExchange);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("stocks-by-exchange")]
		public async Task<IActionResult> GetStocksByExchange([FromQuery] string exchangeCode)
		{
			try
			{
				var stocksByExchange = await _CotizacionesRepo.GetStocksMarketsCountry(exchangeCode);

				if (stocksByExchange == null)
					return NotFound();

				if (!stocksByExchange.Any())
					return Ok("No hay elementos para mostrar");

				return Ok(stocksByExchange);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("indexes-constituens")]
		public async Task<IActionResult> GetIndexesConstituens([FromQuery] string symbol)
		{
			try
			{
				var indexesCons = await _CotizacionesRepo.GetIndexesConstituens(symbol);

				if (indexesCons == null)
					return NotFound();

				if (!indexesCons.Any())
					return Ok("Sin elementos para mostar");

				return Ok(indexesCons);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
