using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class IndexConstituent
	{
		public string Ticker { get; set; }
		public string Code { get; set; }
		public string Exchange { get; set; }
		public string Name { get; set; }
		public string Sector { get; set; }
		public string Industry { get; set; }

		public IndexConstituent(string ticker, string code, string exchange, string name, string sector, string industry)
		{
			Ticker = ticker;
			Code = code;
			Exchange = exchange;
			Name = name;
			Sector = sector;
			Industry = industry;
		}
	}
}
