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
		Task<IEnumerable<Cotizacion>> ObtenerCotizacionPorMoneda(int idMoneda);
		Task AgregarAsyc(Cotizacion cotizacion);
	}
}
