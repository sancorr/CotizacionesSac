using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CotizacionesDomain.Entities
{
	public class Cotizacion
	{
		public int Id { get; set; }
		public int MonedaId { get; set; }
		public decimal ValorCompra { get; set; }
		public decimal ValorVenta { get; set; }
		public DateTime FechaCotizacion { get; set; }


		//Constructor
		public Cotizacion(int id, int idMoneda, decimal valorCompra, decimal valorVenta, DateTime fecha)
		{
			Id = id;
			MonedaId = idMoneda;
			ValorCompra = valorCompra;
			ValorVenta = valorVenta;
			FechaCotizacion = fecha;
		}
	}
}
