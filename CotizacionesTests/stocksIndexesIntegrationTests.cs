using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CotizacionesApi;
using CotizacionesDomain.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;


namespace CotizacionesTests
{
	public class stocksIndexesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		//Cliente http para realizar peticiones
		private readonly HttpClient _httpClient;
		//Con esto evito instanciar WAF en cada test, no repito codigo y mejoro el rendimiento.
		public stocksIndexesIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_httpClient = factory.CreateClient();
		}

		string controllerPath = "api/markets/";

		[Fact]
		public async Task GetStocksCodes_RetornaCodigosMercadosAcciones()
		{
			var response = await _httpClient.GetStringAsync("api/markets/stocks-codes");
			var codes = JsonConvert.DeserializeObject<List<MarketCode>>(response);
			Assert.IsType<List<MarketCode>>(codes);
			Assert.NotEmpty(codes);
			Assert.NotNull(codes);

		}

		[Fact]
		public async Task GetIndexesCodes_RetornaIndexesMarketsCodes() 
		{
			var response = await _httpClient.GetStringAsync(controllerPath + "indexes-codes");
			var codes = JsonConvert.DeserializeObject<List<MarketCode>>(response);
			Assert.IsType<List<MarketCode>>(codes);
			Assert.NotEmpty(codes);
			Assert.NotNull(codes);
		}

		[Fact]
		public async Task GetAllIndexes_RetornaAllIndexes() 
		{
			var response = await _httpClient.GetStringAsync(controllerPath + "all-indexes");
			var indexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(response);
			Assert.IsType<List<FinancialInstrument>>(indexes);
			Assert.NotEmpty(indexes);
			Assert.NotNull(indexes);
		}

		[Fact]
		public async Task GetUsaIndexes_RetornaUsaIndexes()
		{
			var response = await _httpClient.GetStringAsync(controllerPath + "usa-indexes");
			var usaIndexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(response);
			Assert.IsType<List<FinancialInstrument>>(usaIndexes);
			Assert.NotEmpty(usaIndexes);
			Assert.NotNull(usaIndexes);
		}

		[Theory]
		[InlineData("STU")] //Probar con parametro incorrecto para que devuelva mensaje controlado de error
		public async Task GetIndexesMarketCountry_RetornaIndexesCountries(string code)
		{
			var response = await _httpClient.GetStringAsync($"{controllerPath}indexes-by-exchange?exchangeCode={code}");
			var indexes = JsonConvert.DeserializeObject<List<FinancialInstrument>>(response);

			if (indexes.Count <= 0)
				Assert.Fail("La respuesta no contiene un JSON valido o es una lista vacia");

			Assert.IsType<List<FinancialInstrument>>(indexes);
			Assert.NotNull(indexes);
			Assert.True(indexes.Count > 0);

		}

		[Theory]
		[InlineData("US")] //Probar con parametro incorrecto para que devuelva mensaje controlado de error
		public async Task GetStocksMarketCountry_RetornaStocksCountries(string code)
		{
			var response = await _httpClient.GetStringAsync($"{controllerPath}stocks-by-exchange?exchangeCode={code}");
			var stocks = JsonConvert.DeserializeObject<List<FinancialInstrument>>(response);

			if (stocks.Count <= 0)
				Assert.Fail("La respuesta no contiene un JSON valido o es una lista vacia");

			Assert.IsType<List<FinancialInstrument>>(stocks);
			Assert.NotNull(stocks);
			Assert.True(stocks.Count > 0);

		}

		[Theory]
		[InlineData("NDX")] //verificar el error de recibir mal un parametro, o un parametro mal formado
		public async Task GetIndexesConstituens_RetornaIndexesConstituens(string symbol)
		{
			var response = await _httpClient.GetStringAsync($"{controllerPath}indexes-constituens?symbol={symbol}");
			var indexconstituent = JsonConvert.DeserializeObject<List<IndexConstituent>>(response);

			if (indexconstituent.Count <= 0)
				Assert.Fail("La respuesta no es JSON o es una lista vacia, corregir parametro");
			Assert.IsType<List<IndexConstituent>>(indexconstituent);
			Assert.NotNull(indexconstituent);
			Assert.True(indexconstituent.Count > 0);

		}

		[Theory]
		[InlineData("NDX")] //verificar el error de recibir mal un parametro, o un parametro mal formado
		public async Task GetLastQuote_RetornaLastQuote(string symbol)
		{
			var response = await _httpClient.GetStringAsync($"{controllerPath}last-quote?symbol={symbol}");
			var lastQuote = JsonConvert.DeserializeObject<LastQuote>(response);

			
			Assert.IsType<LastQuote>(lastQuote);
			Assert.NotEmpty(response);
			Assert.NotNull(lastQuote);

		}
	}
}
