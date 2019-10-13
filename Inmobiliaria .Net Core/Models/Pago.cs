using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
    public class Pago
    {
		[Key]
        public int IdPago { get; set; }

        public int NroPago { get; set; }

        public int IdAlquiler { get; set; }

        public Alquiler Alquiler { get; set; }

        public string Fecha { get; set; }

        public decimal Importe { get; set; }
    }
}
