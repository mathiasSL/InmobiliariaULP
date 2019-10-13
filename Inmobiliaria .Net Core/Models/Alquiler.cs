using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public class Alquiler
	{
        [Key]
        public int IdAlquiler { get; set; }

        public decimal Importe { get; set; }

        public string FechaInicio { get; set; }

        public string FechaFin { get; set; }

        public int IdInquilino { get; set; }

        public int IdInmueble { get; set; }

        public Inquilino Inquilino { get; set; }

		public Inmueble Inmueble { get; set; }

		
	}
}
