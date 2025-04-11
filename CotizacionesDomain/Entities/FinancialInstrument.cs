using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class FinancialInstrument
	{
		public string Ticker { get; set; }
		public string Symbol { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Currency { get; set; }
		public string Country { get; set; }
		public string Exchange { get; set; }


		public FinancialInstrument(string ticker, string symbol, string name, string type, string currency,
			string country, string exchange)
		{
			Ticker = ticker;
			Symbol = symbol;
			Name = name;
			Type = type;
			Currency = currency;
			Country = country;
			Exchange = exchange;
		}
	}
}
