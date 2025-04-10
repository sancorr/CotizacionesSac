using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class LastQuote
	{
		[JsonProperty("ticker")]
		public string Ticker { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("symbol")]
		public string Symbol { get; set; }
		[JsonProperty("price")]
		public decimal Price { get; set; }
		[JsonProperty("previous_close")]
		public decimal PreviousClose { get; set; }
		[JsonProperty("daily_price_change")]
		public decimal DailyPriceChange { get; set; }
		[JsonProperty("daily_percentage_change")]
		public decimal DailyPercentageChange { get; set; }
		[JsonProperty("timestamp")]
		public long Timestamp { get; set; }
		[JsonProperty("asset_class")]
		public string AssetClass { get; set; }
		[JsonProperty("currency")]
		public string Currency { get; set; }
		[JsonProperty("logo_url")]
		public string LogoUrl { get; set; }
		[JsonProperty("volume")]
		public long Volume { get; set; }
		[JsonProperty("broker")]
		public string Broker { get; set; }
		[JsonProperty("ohlc_week")]
		public OHLC_Weekly OhlcWeek { get; set; }
	}

	public class OHLC_Weekly
	{
		[JsonProperty("open")]
		public decimal Open { get; set; }
		[JsonProperty("high")]
		public decimal High { get; set; }
		[JsonProperty("low")]
		public decimal Low { get; set; }
		[JsonProperty("close")]
		public decimal Close { get; set; }
	}
}
