using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class LastQuote
	{
		public string Ticker { get; set; }
		public string Name { get; set; }
		public string Symbol { get; set; }
		public decimal Price { get; set; }
		public decimal PreviousClose { get; set; }
		public decimal DailyPriceChange { get; set; }
		public decimal DailyPercentageChange { get; set; }
		public long Timestamp { get; set; }
		public string AssetClass { get; set; }
		public string Currency { get; set; }
		public string LogoUrl { get; set; }
		public long Volume { get; set; }
		public string Broker { get; set; }
		public OHLC_Weekly OhlcWeek { get; set; }
	}

	public class OHLC_Weekly
	{
		public decimal Open { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Close { get; set; }
	}
}
