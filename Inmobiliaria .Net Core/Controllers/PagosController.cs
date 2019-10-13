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
            return View(lista);
        }

		public ActionResult Create()
		{
			ViewBag.pago = repoPago.ObtenerTodos();
			ViewBag.alquiler = repoAlquiler.ObtenerTodos();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Pago pago)
		{
			try
			{
			    repoPago.Alta(pago);
				TempData["Id"] = "Se creó correctamente";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.pago = repoPago.ObtenerTodos();
				ViewBag.alquiler = repoAlquiler.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(pago);
				//return RedirectToAction(nameof(Index));
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
				TempData["Id"] = "";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(entidad);
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
				TempData["Id"] = "";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(entidad);
			}
		}

	}
}