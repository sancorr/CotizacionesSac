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
	public class StocksInedexesUnitTests
	{
		//Test unitarios con moq de instrumentos financieros
		[Fact]
		public async Task GetStocksCodes_RetornaCodeMarkets()
		{
			//1 - crear mock de interface
			var moq = new Mock<ICotizacionesRepository>();
			//2 - Llamado al metodo de la interface (el moq) - si es una lista, debe retornar una lista de objetos. Este moq simula una respuesta correcta.
			moq.Setup(repo => repo.GetStocksMarketsCode()).ReturnsAsync(new List<MarketCode> {
				new MarketCode("Argentina", "ARG"),
				new MarketCode("Brasil","BR"),			
				new MarketCode("República checa","CHK")			
			});
			//3 - Inyectar el moq que simula una respusta al controlador que hace la peticion
			// Al controlador que hace la peticion le paso el moq como objeto
			var controller = new CotizacionesController(moq.Object);
			//4 - uso el método desde el controlador (el metodo es asincrono originalmente)
			var result = await controller.GetStocksCodes() as OkObjectResult;
			// Accedo al value de la respuesta del controlador, contemplo null con "?"
			var codesList = result?.Value as List<MarketCode>;

			//5- verificacion de resultados, usando clase Assert y sus métodos
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotEmpty(codesList);
		}

		[Fact]
		public async Task GetIndexesCodes_RetornaIndexesCodeMarkets()
		{
			//1 - crear mock de interface
			var moq = new Mock<ICotizacionesRepository>();
			//2 - Llamado al metodo de la interface (el moq) - si es una lista, debe retornar una lista de objetos. Este moq simula una respuesta correcta.
			moq.Setup(repo => repo.GetIndexesMarketsCode()).ReturnsAsync(new List<MarketCode> {
				new MarketCode("ArgIndex","ARGINDX"),
				new MarketCode("BrasilIndex","BRINDX"),
				new MarketCode("UruguayIndex","UYINDX")
			});
			//3 - Inyectar el moq que simula una respusta al controlador que hace la peticion
			// Al controlador que hace la peticion le paso el moq como objeto
			var controller = new CotizacionesController(moq.Object);
			//4 - uso el método desde el controlador (el metodo es asincrono originalmente)
			var result = await controller.GetIndexesCodes() as OkObjectResult;
			// Accedo al value de la respuesta del controlador, contemplo null con "?"
			var codesList = result?.Value as List<MarketCode>;

			//5- verificacion de resultados, usando clase Assert y sus métodos
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotEmpty(codesList);
		}

		[Fact]
		public async Task GetAllIndexes_RetornaTodosLosIndicesCorrectos()
		{
			var moq = new Mock<ICotizacionesRepository>();

			moq.Setup(repo => repo.GetAllIndexes()).ReturnsAsync(new List<FinancialInstrument>{ 
				new FinancialInstrument("ARG.INDX","ARGINDX", "ArgenIndex", "Index", "USD", "Argentina", "USD/ARS"),
				new FinancialInstrument("UY.INDX", "UYINDX", "UruguayIndex", "Index", "USD", "Uruguay", "USD"),
				new FinancialInstrument("BR.INDX", "BRINDX", "BrasilIndex", "Index", "USD", "Brasil", "USD")
			});

			var controller = new CotizacionesController(moq.Object);

			var result = await controller.GetAllIndexes() as OkObjectResult;

			var codesList = result?.Value as List<FinancialInstrument>;

			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotEmpty(codesList);
		}

		[Fact]
		public async Task GetUsaIndexes_RetornaIndexesUsa()
		{
			var moq = new Mock<ICotizacionesRepository>();

			moq.Setup(repo => repo.GetUsaIndexes()).ReturnsAsync(new List<FinancialInstrument> { 
				new FinancialInstrument("NDX.INDX","NasdaqINDX", "Nasdaq100", "Index", "USD", "US", "USD"),
				new FinancialInstrument("GSPC.INDX","S&P500INDX", "S&P500", "Index", "USD", "US", "USD"),
				new FinancialInstrument("DJS.INDX","D&JINDX", "DOW&JONES", "Index", "USD", "US", "USD"),
			});

			var controller = new CotizacionesController(moq.Object);
			var result = await controller.GetUsaIndexes() as OkObjectResult;
			var usaIndexesList = result?.Value as List<FinancialInstrument>;

			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.NotEmpty(usaIndexesList);
		}

		[Theory]
		[InlineData("US")]
		public async Task GetIndexesMarketCountry_RetornaCodigosDeIndices(string code)
		{
			var moq = new Mock<ICotizacionesRepository>();

			moq.Setup(repo => repo.GetIndexesMarketCountry(code)).ReturnsAsync(new List<FinancialInstrument>
			{ 
				new FinancialInstrument("NDX.INDX","NasdaqINDX", "Nasdaq100", "Index", "USD", "USA", "USD"),
				new FinancialInstrument("GSPC.INDX","GSPCINDX", "S&P500", "Index", "USD", "USA", "USD")
			});

			var controller = new CotizacionesController(moq.Object);

			var result = await controller.GetIndexesByExchange(code);
			//Verifica que sea el tipo que espero
			//Espero un OkObjectResult de result
			var okResult = Assert.IsType<OkObjectResult>(result);
			//verifica que se el tipo que espero
			//espero que sea una lista de FinancialInstrumen de la respuesta que guarda okResult del metodo al que llama result
			var data = Assert.IsType<List<FinancialInstrument>>(okResult.Value);

			Assert.Equal(200, okResult.StatusCode);
			Assert.NotNull(okResult);

			//verificar que el metodo fue llamado 1 vez
			moq.Verify(x => x.GetIndexesMarketCountry(code), Times.Once);
		}

		[Theory]
		[InlineData("US")]
		public async Task GetStocksMarketCountry_RetornaAccionesDeUnMercado(string code)
		{
			//ARRANGE
			var moq = new Mock<ICotizacionesRepository>();

			var expectedData = new List<FinancialInstrument>
			{
				new FinancialInstrument("BCR.STOCK","Banco Nacion", "Banco Argentino", "Stock", "Peso", "ARG", "ARS"),
				new FinancialInstrument("APL.Stock","APPLE", "APPLE", "Stock", "USD", "USA", "USD")
			};

			moq.Setup(repo => repo.GetStocksMarketsCountry(code)).ReturnsAsync(expectedData);

			var controller = new CotizacionesController(moq.Object);
			//ACT
			var result = await controller.GetStocksByExchange(code);
			//ASSERT
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualData = Assert.IsType<List<FinancialInstrument>>(okResult.Value);

			Assert.NotNull(okResult);
			Assert.NotEmpty(actualData);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(expectedData.Count, actualData.Count);
			Assert.Equal(expectedData[0].Ticker, actualData[0].Ticker);

			moq.Verify(x => x.GetStocksMarketsCountry(code), Times.Once);
		}

		[Theory]
		[InlineData("NDX")]
		public async Task GetIndexesConstituens_RetornaConstituyentesDeUnIndice(string symbol) {
			//ARRANGE
			var moq = new Mock<ICotizacionesRepository>();
			var expectedData = new List<IndexConstituent>
			{
				new IndexConstituent("APL.USA", "APL", "USA", "APPLE", "Technology", "Devices"),
				new IndexConstituent("MICROSOFT.USA", "MSC", "USA", "MICROSOFT", "Technology", "Software")
			};

			moq.Setup(repo => repo.GetIndexesConstituens(symbol)).ReturnsAsync(expectedData);

			var controller = new CotizacionesController(moq.Object); //Inyeccion de dependencia
			//ACT
			var result = await controller.GetIndexesConstituens(symbol);
			//ASSERT
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualData = Assert.IsType<List<IndexConstituent>>(okResult.Value);
			Assert.NotNull(okResult);
			Assert.Equal(expectedData.Count, actualData.Count);
			Assert.Equal(expectedData[0].Name, actualData[0].Name);

			moq.Verify(x => x.GetIndexesConstituens(symbol), Times.Once);
		}

		[Theory]
		[InlineData("NDX")]
		public async Task GetLastQuote_RetornaUltimaCotizacion(string symbol) {
			//ARRANGE
			var moq = new Mock<ICotizacionesRepository>();
			var expectedData = new LastQuote("USD.CURRENCY", "DOLAR", "USD", (decimal)1355.50, (decimal)1350.25, (decimal)1.2, (decimal)1.5, 156879851358, "USD.CRCY", "USD", "logo_url.com/dolar", 123456789, "bull market", new OHLC_Weekly((decimal)123.3, (decimal)127.3, (decimal)102.3, (decimal)125.3));

			moq.Setup(repo => repo.GetLastQuote(symbol)).ReturnsAsync(expectedData);
			var controller = new CotizacionesController(moq.Object);
			//ACT
			var result = await controller.GetLastQuoteInstrument(symbol);
			//ASSERT
			var okResult = Assert.IsType<OkObjectResult>(result);
			var actualData = Assert.IsType<LastQuote>(okResult.Value);
			Assert.NotNull(okResult);
			Assert.Equal(expectedData, actualData);
			Assert.Equal(expectedData.DailyPriceChange, actualData.DailyPriceChange);

			moq.Verify(x => x.GetLastQuote(symbol), Times.Once);
		}
	}
}
