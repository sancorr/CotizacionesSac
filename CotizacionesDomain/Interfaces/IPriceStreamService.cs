using CotizacionesDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Interfaces
{
    public interface IPriceStreamService
    {
        Task SubscribeAsync(IEnumerable<string> Tickers);
        Task UnsubscribeAsync(IEnumerable<string> Tickers);

        event Action<PriceData> OnPriceRecieved;
    }
}
