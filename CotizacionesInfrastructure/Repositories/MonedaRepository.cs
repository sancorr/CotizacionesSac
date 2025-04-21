using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CotizacionesDomain.Entities;
using CotizacionesDomain.Interfaces;
using Newtonsoft.Json;

namespace CotizacionesInfrastructure.Repositories
{
	//CAPA DE NEGOCIO ---> implementa los metodos que la Interface dice que debe implementar.
	public class MonedaRepository : IMonedaRepository
	{
		private readonly HttpClient _httpClient;

		//Constructor de http
		public MonedaRepository(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		//LLAMADOS A API - COTIZACIONES DOLARES
		public async Task<IEnumerable<Moneda>> ObtenerDolaresAsync()
		{
			try
			{
				string response = await _httpClient.GetStringAsync("https://dolarapi.com/v1/dolares");

				var monedas = JsonConvert.DeserializeObject<List<Moneda>>(response);
				return monedas;
			}
			catch (HttpRequestException ex)
			{
				throw new Exception($"Error de red o HTTP. Error: {ex.Message}", ex);
			}
			catch (TaskCanceledException ex)
			{
				throw new Exception($"Timeout al intentar obtener datos. Error: {ex.Message}", ex);
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error al deserializar datos. Error: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
			}
		}

		//LLAMADOS A API - COTIZACIONES EURO
		public async Task<Moneda> ObtenerEuros()
		{
			try
			{
				string response = await _httpClient.GetStringAsync("https://dolarapi.com/v1/cotizaciones/eur");

				var euros = JsonConvert.DeserializeObject<Moneda>(response);
				return euros;
			}
			catch (HttpRequestException ex)
			{
				throw new Exception($"Error de red o HTTP. Error: {ex.Message}", ex);
			}
			catch (TaskCanceledException ex)
			{
				throw new Exception($"Timeout al intentar obtener datos. Error: {ex.Message}", ex);
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error al deserializar datos. Error: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
			}
		}

		//LLAMADOS A API - COTIZACIONES REAL BRASILERO
		public async Task<Moneda> ObtenerReal()
		{
			try
			{
				string response = await _httpClient.GetStringAsync("https://dolarapi.com/v1/cotizaciones/brl");

				var real = JsonConvert.DeserializeObject<Moneda>(response);
				return real;
			}
			catch (HttpRequestException ex)
			{
				throw new Exception($"Error de red o HTTP. Error: {ex.Message}", ex);
			}
			catch (TaskCanceledException ex)
			{
				throw new Exception($"Timeout al intentar obtener datos. Error: {ex.Message}", ex);
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error al deserializar datos. Error: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
			}
		}

		//LLAMADOS A API - COTIZACIONES CHILENO
		public async Task<Moneda> ObtenerChileno()
		{
			try
			{
				string respone = await _httpClient.GetStringAsync("https://dolarapi.com/v1/cotizaciones/clp");
				
				var chileno = JsonConvert.DeserializeObject<Moneda>(respone);
				return chileno;
			}
			catch (HttpRequestException ex)
			{
				throw new Exception($"Error de red o HTTP. Error: {ex.Message}", ex);
			}
			catch (TaskCanceledException ex)
			{
				throw new Exception($"Timeout al intentar obtener datos. Error: {ex.Message}", ex);
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error al deserializar datos. Error: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
			}
		}

		//LLAMADOS A API - COTIZACIONES URUGUAYO
		public async Task<Moneda> ObtenerUruguayo()
		{
			try
			{
				string respone = await _httpClient.GetStringAsync("https://dolarapi.com/v1/cotizaciones/uyu");

				var uruguayo = JsonConvert.DeserializeObject<Moneda>(respone);
				return uruguayo;
			}
			catch (HttpRequestException ex)
			{
				throw new Exception($"Error de red o HTTP. Error: {ex.Message}", ex);
			}
			catch (TaskCanceledException ex)
			{
				throw new Exception($"Timeout al intentar obtener datos. Error: {ex.Message}", ex);
			}
			catch (JsonException ex)
			{
				throw new Exception($"Error al deserializar datos. Error: {ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error inesperado. Error: {ex.Message}", ex);
			}
		}
	}
}
