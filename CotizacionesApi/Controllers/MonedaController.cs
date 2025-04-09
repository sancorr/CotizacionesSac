using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CotizacionesDomain.Interfaces;

namespace CotizacionesApi.Controllers
{
	[ApiController] //Valida que sea una controlador y valida los datos de entradas
	[Route("api/monedas")] //Define la ruta base del controlador
	public class MonedaController : ControllerBase //Controller base maneja peticiones HTTP sin vistas
	{
		private readonly IMonedaRepository _monedaRepository; //objeto de repositorio (interfaz con metodos y clase heredada que los implementa)

		public MonedaController(IMonedaRepository monedaRepositorio)
		{
			_monedaRepository = monedaRepositorio;
		}

		[HttpGet("dolares")]
		public async Task<IActionResult> obtenerDolares()
		{
			try
			{
				var monedas = await _monedaRepository.ObtenerDolaresAsync();

				if (monedas == null)
					return NotFound();
				if (!monedas.Any())
					return Ok("No hay elementos que mostrar");


				return Ok(monedas);

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

		[HttpGet("euro")]
		public async Task<IActionResult> obtenerEuro()
		{
			try
			{
				var moneda = await _monedaRepository.ObtenerEuros();
				return Ok(moneda);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("real")]
		public async Task<IActionResult> obtenerReal()
		{
			try
			{
				var moneda = await _monedaRepository.ObtenerReal();

				if (moneda == null)
					return NotFound();

				return Ok(moneda);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("chl")]
		public async Task<IActionResult> obtenerChileno()
		{
			try
			{
				var moneda = await _monedaRepository.ObtenerChileno();
				if (moneda == null)
					return NotFound();

				return Ok(moneda);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		[HttpGet("uy")]
		public async Task<IActionResult> obtenerUruguayo()
		{
			try
			{
				var moneda = await _monedaRepository.ObtenerUruguayo();
				if (moneda == null)
					return NotFound();

				return Ok(moneda);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
