using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CotizacionesDomain.Entities;
using CotizacionesDomain.Interfaces;
using CotizacionesApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace CotizacionesTests
{
	public class MonedaControllerTest
	{
		//Test unitario para obtenerEuro()
		[Fact] //Indico que es una prueba unitaria
		public async Task ObtenerEuro_RetornaValorCorrecto() {

			var mockInterface = new Mock<IMonedaRepository>(); //Mock de la interfaz
			mockInterface.Setup(repo => repo.ObtenerEuros()).ReturnsAsync(new Moneda( //Llamado al metodo que establece la interfaz
				(decimal)125.36,
				(decimal)325.69,
				"Oficial",
				"Euro",
				"EUR", 
				DateTime.Now));

			var controller = new MonedaController(mockInterface.Object); //instancia de la clase que tiene el controlador, inyectando el mock recien creado

			var resultado = await controller.obtenerEuro() as OkObjectResult;

			var moneda = resultado?.Value as Moneda; //extaigo la moneda de la respuesta convirtiendo la respuesta en una instacia de moneda

			//verificaciones de resultados

			Assert.NotNull(resultado);
			Assert.Equal(200, resultado.StatusCode);
			Assert.NotNull(moneda);
			Assert.Equal("Euro", moneda.Nombre);
			Assert.Equal((decimal)125.36, moneda.Compra);
			Assert.Equal((decimal)325.69, moneda.Venta);
		}

		[Fact] //--> Indica que es una prueba unitaria
		public async Task ObtenerDolares_ValoresCorrectos()
		{
			//Mock que simula el consumo de la info
			var mockInterface = new Mock<IMonedaRepository>();
			mockInterface.Setup(repo => repo.ObtenerDolaresAsync())
				.ReturnsAsync(new List<Moneda> {
					new Moneda((decimal)360.5,(decimal)365.5, "Oficial", "USD_OF", "$", DateTime.Now),
					new Moneda((decimal)1360.5,(decimal)1365.5, "Blue", "USD_BLUE", "$", DateTime.Now),
					new Moneda((decimal)60.5,(decimal)65.5, "Bolsa", "USD_BOLSA", "$", DateTime.Now),
					new Moneda((decimal)560.5,(decimal)565.5, "Mep", "USD_MEP", "$", DateTime.Now),
					new Moneda((decimal)360.5,(decimal)365.5, "CCL", "USD_CCL", "$", DateTime.Now),
					new Moneda((decimal)360.5,(decimal)365.5, "Tarjeta", "USD_TAR", "$", DateTime.Now),
				});

			//inyeccion del mock al controlador
			var controller = new MonedaController(mockInterface.Object);
			//uso del metodo desde el controlador
			var result = await controller.obtenerDolares() as OkObjectResult;
			//Evaluo el resultado de la respuesta de controlador, contemplando nulos
			var dolares = result?.Value as List<Moneda>;

			//verificacion de la prueba- assert
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotEmpty(dolares);


		}

		[Fact]
		public async Task ObtenerReal_ValoresCorrectos() {

			//mock
			var mockInterface = new Mock<IMonedaRepository>();
			mockInterface.Setup(repo => repo.ObtenerReal()).ReturnsAsync(new Moneda(
				(decimal)159.9,
				(decimal)160.12,
				"OFICIAL",
				"RLS",
				"$",
				DateTime.Now));

			//Action
			var controller = new MonedaController(mockInterface.Object);
			var result = await controller.obtenerReal() as OkObjectResult;
			var real = result?.Value as Moneda;

			//result
			Assert.NotNull(result);
			Assert.NotNull(real);
			Assert.Equal(200, result.StatusCode);
			Assert.Equal((decimal)159.9, real.Compra);
			Assert.Equal((decimal)160.12, real.Venta);
		}


		[Fact]
		public async Task ObtenerChileno_ValoresCorrectos() {

			var mockInterface = new Mock<IMonedaRepository>();
			mockInterface.Setup(repo => repo.ObtenerChileno()).ReturnsAsync(new Moneda(
				(decimal)1.2,
				(decimal)1.3,
				"Oficial",
				"Chileno",
				"CHL",
				DateTime.Now));

			var controller = new MonedaController(mockInterface.Object);
			var result = await controller.obtenerChileno() as OkObjectResult;
			var moneda = result?.Value as Moneda;

			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotNull(moneda);
			Assert.Equal((decimal)1.2, moneda.Compra);
			Assert.Equal((decimal)1.3, moneda.Venta);
		}


		[Fact]
		public async Task ObtenerUruguayo_ValoresCorrectos() {
			var mockInterface = new Mock<IMonedaRepository>();
			mockInterface.Setup(repo => repo.ObtenerUruguayo()).ReturnsAsync(new Moneda(
				(decimal)26.06,
				(decimal)28,
				"Oficial",
				"Uruguayo",
				"UY",
				DateTime.Now)
				);

			var controller = new MonedaController(mockInterface.Object);
			var result = await controller.obtenerUruguayo() as OkObjectResult;
			var moneda = result?.Value as Moneda;

			Assert.NotNull(result);
			Assert.NotNull(moneda);
			Assert.Equal(200, result.StatusCode);
			Assert.Equal((decimal)26.06, moneda.Compra);
			Assert.Equal(28, moneda.Venta);
		}
	}
}
