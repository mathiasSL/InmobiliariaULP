using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Models
{
	public interface IRepositorioAlquiler : IRepositorio<Alquiler>
	{
		int CambioDisponible(int p);
	}
}
