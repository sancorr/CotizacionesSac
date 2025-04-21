using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CotizacionesDomain.Entities;
using CotizacionesDomain.Interfaces;
using CotizacionesInfrastructure.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CotizacionesInfrastructure.Repositories
{
	public class MarketCodesRepositories : ICotizacionesRepository
	{
		private readonly HttpClient _httpClient; 
		private readonly string token;

		public MarketCodesRepositories(HttpClient httpClient)
		{
			_httpClient = httpClient;
			token = ProfitApiSettingsLoader.LoadToken();
		}

		// 1- Obtener los codigos de mercados de acciones (stocks)
		public async Task<List<MarketCode>> GetStocksMarketsCode()
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/exchanges?token={token}&type=stocks");

				if (string.IsNullOrEmpty(response))
					throw new Exception("La respuesta de la API no contiene datos.");

				var codes = JsonConvert.DeserializeObject<List<MarketCode>>(response);
				return codes;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		// 2- Obtener la lista de codigos de mercados de indices
		public async Task<List<MarketCode>> GetIndexesMarketsCode()
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/exchanges?token={token}&type=indexes");

				if (string.IsNullOrEmpty(response))
					throw new Exception("La respuesta de la API no contiene datos.");

				var codes = JsonConvert.DeserializeObject<List<MarketCode>>(response);
				return codes;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		//3-Obtener TODOS los indices soportados por la API - de aca obtengo el symbol para el 7
		public async Task<List<FinancialInstrument>> GetAllIndexes()
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/indices?token={token}&skip=0&limit=1000");

				var jsonObjetc = JObject.Parse(response);
				var dataArray = jsonObjetc["data"].ToString();

				if (string.IsNullOrEmpty(dataArray))
					throw new Exception("La respuesta de la API no contiene datos de índices.");

				var indexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(dataArray);
				return indexes;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		// 4 - Obtener la lista de indices de EEUU - Tambien de aca puedo darle el SYMBOL al 7
		public async Task<List<FinancialInstrument>> GetUsaIndexes()
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/indices?token={token}&limit=1000&exchange=INDX&country=United%20States&currency=USD&available_data=historical&available_data=fundamental&type=INDEX");

				var jsonObject = JObject.Parse(response);
				var dataArray = jsonObject["data"].ToString();

				if (string.IsNullOrEmpty(dataArray))
					throw new Exception("La respuesta de la API no contiene datos de índices.");

				var usaIndexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(dataArray);
				return usaIndexes;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		//5 - Obtener Lista de indices de paises segun codigos obtenidos en el 2
		//PENDIENTE : Automatizacion para insertar un codigo recorriendo la lista de codigos obtenidos en el paso 2 e insertarla en esta ruta para que devuelva los indices de un mercado especifico
		public async Task<List<FinancialInstrument>> GetIndexesMarketCountry(string exchangeCode)
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/indices?token={token}&skip=0&limit=1000&exchange={exchangeCode}&available_data=historical&available_data=fundamental&type=INDEX");

				var jsonObject = JObject.Parse(response);
				var dataArray = jsonObject["data"].ToString();

				if (string.IsNullOrEmpty(dataArray))
					throw new Exception("La respuesta de la API no contiene datos de índices.");

				var indexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(dataArray);
				return indexes;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		//6 - Obtener una lista de acciones de un mercado especifico, bajo un codigo obtenido en el 1
		//PENDIENTE : Misma automatizacion del punto 4, pero con codigos del punto 1
		public async Task<List<FinancialInstrument>> GetStocksMarketsCountry(string exchangeCode)
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/reference/stocks?token={token}&exchange={exchangeCode}&limit=1000");

				var jsonObject = JObject.Parse(response);
				var dataArray = jsonObject["data"].ToString();

				if (string.IsNullOrEmpty(dataArray))
					throw new Exception("La respuesta de la API no contiene datos.");

				var stocks = JsonConvert.DeserializeObject<List<FinancialInstrument>>(dataArray);
				return stocks;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		//7 - Constituyentes de un indice
		//Donde SYMBOL, es el symbol de un indice determinado, obtenido en la consulta que devuelve indices
		//PENDIENTE: Automatizacion para insertar SYMBOL dinamicamente

		public async Task<List<IndexConstituent>> GetIndexesConstituens(string symbol)
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/fundamentals/indexes/index_constituents/{symbol}?token={token}");

				if (string.IsNullOrEmpty(response))
					throw new Exception("La respuesta de la API no contiene datos.");

				var indexConstituentDictionary = JsonConvert.DeserializeObject<Dictionary<string, IndexConstituent>>(response);

				var indexConstituentList = indexConstituentDictionary.Values.ToList();
				return indexConstituentList;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}

		// 8 - Obtener la ultima cotizacion de un elemento, recibe el Symbol de una accion y el Ticker de un indice
		public async Task<LastQuote> GetLastQuote(string symbol)
		{
			try
			{
				string response = await _httpClient.GetStringAsync($"https://api.profit.com/data-api/market-data/quote/{symbol}?token={token}");

				if (string.IsNullOrEmpty(response))
					throw new Exception("La respuesta de la API no contiene datos.");

				var lastQuote = JsonConvert.DeserializeObject<LastQuote>(response);

				return lastQuote;
			}
			catch (HttpRequestException ex)
			{
				//Error de red,servidor caido, timeout
				throw new Exception("No se pudo contectar al servidor. Error: ", ex);
			}
			catch (JsonException ex)
			{
				//Error de parseo
				throw new Exception("No se pudo interpretar la respuesta del servidor.Error: ", ex);
			}
			catch (Exception ex)
			{
				//Errores generales, por ejemplo de protocolo http (no se capturan automaticamente, por eso muestro el mensaje)
				throw new Exception("Ocurrió un error inesperado. Error: ", ex);
			}
		}
	}
}
