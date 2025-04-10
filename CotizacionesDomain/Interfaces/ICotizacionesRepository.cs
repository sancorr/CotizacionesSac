using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CotizacionesDomain.Entities;

namespace CotizacionesDomain.Interfaces
{
	public interface ICotizacionesRepository
	{
		//Obtener los codigos de mercados de acciones
		Task<List<MarketCode>> GetStocksMarketsCode();
		// 2- Obtener la lista de codigos de mercados de indices
		Task<List<MarketCode>> GetIndexesMarketsCode();
		//3-Obtener TODOS lis indices soportados por la API 
		Task<List<FinancialInstrument>> GetAllIndexes();
		// 4 - Obtener la lista de indices de EEUU
		Task<List<FinancialInstrument>> GetUsaIndexes();
		//5 - Obtener Lista de indices de paises segun codigos obtenidos en el 2
		Task<List<FinancialInstrument>> GetIndexesMarketCountry(string exchangeCode);
		//6 - Obtener una lista de acciones de un mercado especifico, bajo un codigo obtenido en el 1
		Task<List<FinancialInstrument>> GetStocksMarketsCountry(string exchangeCode);
		//7 - Constituyentes de un indice
		Task<List<IndexConstituent>> GetIndexesConstituens(string symbol);
		//8 - Ultima cotizacion de un instrumento
		Task<LastQuote> GetLastQuote(string symbol);
	}
}
