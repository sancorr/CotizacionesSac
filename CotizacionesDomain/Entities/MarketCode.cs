using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class MarketCode
	{
		public string Name { get; set; }
		public string Code { get; set; }

		public MarketCode(string name, string code)
		{
			Name = name;
			Code = code;
		}

	}

	
}
