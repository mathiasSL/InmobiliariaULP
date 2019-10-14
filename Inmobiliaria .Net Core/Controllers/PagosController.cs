using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorio<Pago> repoPago;
        private readonly IRepositorio<Alquiler> repoAlquiler;

        public PagosController(IRepositorio<Pago> repoPago, IRepositorio<Alquiler> repoAlquiler)
        {
            this.repoPago = repoPago;
            this.repoAlquiler = repoAlquiler;
        }

        public ActionResult Index()
        {
            var lista = repoPago.ObtenerTodos();
			if (TempData.ContainsKey("Alta"))
				ViewBag.Alta = TempData["Alta"];
			if (TempData.ContainsKey("Error2"))
				ViewBag.Error2 = TempData["Error2"];
			return View(lista);
        }

		public ActionResult Create()
		{
			//ViewBag.pago = repoPago.ObtenerTodos();
			ViewBag.alquiler = repoAlquiler.ObtenerTodos();

			int resultado = 0;

			foreach (var item in (IList<Alquiler>)ViewBag.alquiler)
			{
				if (!item.IdAlquiler.Equals(" "))
				{
					resultado++;
				}
			}

			if (resultado > 0)
			{
				return View();
			}
			else
			{
				TempData["Error2"] = "No hay contratos de alquileres disponibles";
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Pago pago)
		{
			try
			{
			    repoPago.Alta(pago);
				TempData["Alta"] = "Se creó correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.pago = repoPago.ObtenerTodos();
				ViewBag.alquiler = repoAlquiler.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
				
			}
		}

		public ActionResult Delete(int id)
		{
			var entidad = repoPago.ObtenerPorId(id);
			return View(entidad);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Pago entidad)
		{
			try
			{
				repoPago.Baja(id);
				TempData["Alta"] = "Pago eliminado";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

		public ActionResult Edit(int id)
		{
			var entidad = repoPago.ObtenerPorId(id);
			return View(entidad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Pago entidad)
		{
			try
			{
				entidad.IdPago = id;
				repoPago.Modificacion(entidad);
				TempData["Alta"] = "Datos modificados con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}

	}
}