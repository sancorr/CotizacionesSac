using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using CotizacionesApi;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using CotizacionesDomain.Entities;

namespace CotizacionesTests
{
	//IClassFixtoure es una interfaz que permite crear un servidor en memoria
	public class MonedaControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;

		public MonedaControllerIntegrationTest(WebApplicationFactory<Program> factory)
		{
			//Crea un cliente HTTP para probar la API en un servidor en memoria
			_client = factory.CreateClient();
		}

		[Fact]
		public async Task ObtnerDolares_RetornaCotizacionesReales()
		{
			//Act, llamo a la api de aplicacion por el endpoint del controlador
			var response = await _client.GetStringAsync("/api/monedas/dolares");

			//verifico el estado de larespuesta (exito,200)
			//Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			//Leo como string y deserializo el objeto lista de dolares
			//var contentResponse = await response.Content.ReadAsStringAsync();


			var dolares = JsonConvert.DeserializeObject<List<Moneda>>(response);
			//verificaciones de la respuesta
			Assert.NotEmpty(dolares);


		}

		[Fact]
		public async Task ObtenerEuro_RetornaCotizacionReal()
		{
			string response = await _client.GetStringAsync("api/monedas/euro");
			
			var euro = JsonConvert.DeserializeObject<Moneda>(response);

			Assert.NotNull(response);
			Assert.NotNull(euro);
		}

		[Fact]
		public async Task ObtenerReal_RetornaCotizacionReal()
		{
			string response = await _client.GetStringAsync("api/monedas/real");

			var real = JsonConvert.DeserializeObject<Moneda>(response);

			Assert.NotNull(response);
			Assert.NotNull(real);
		}

		[Fact]
		public async Task ObtenerChileno_RetornaCotizacionReal()
		{
			string response = await _client.GetStringAsync("api/monedas/chl");

			var chileno = JsonConvert.DeserializeObject<Moneda>(response);

			Assert.NotNull(response);
			Assert.NotNull(chileno);
		}

		[Fact]
		public async Task ObtenerUY_RetornaCotizacionReal()
		{
			string response = await _client.GetStringAsync("api/monedas/uy");

			var uruguayo = JsonConvert.DeserializeObject<Moneda>(response);

			Assert.NotNull(response);
			Assert.NotNull(uruguayo);
		}

	}
}
