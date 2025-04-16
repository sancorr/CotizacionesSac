using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
    public class PriceData
    {
        public string Ticker { get; set; }
        public decimal Price { get; set; }
        public long Timestamp { get; set; }
        public decimal Volume { get; set; }
    }
}
