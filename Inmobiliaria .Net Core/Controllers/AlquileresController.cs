using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class AlquileresController : Controller
    {
        private readonly IRepositorioAlquiler repositorio;
		private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorio<Inquilino> repoInquilino;

        public AlquileresController(IRepositorioAlquiler repositorio, IRepositorioInmueble repoInmueble, IRepositorio<Inquilino> repoInquilino)
        {
            this.repositorio = repositorio;
            this.repoInmueble = repoInmueble;
            this.repoInquilino = repoInquilino;
        }
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            //if (TempData.ContainsKey("IdAlquiler"))
            //    ViewBag.Id = TempData["IdAlquiler"];
            return View(lista);
        }

        public ActionResult Create()
        {
            ViewBag.inmueble = repoInmueble.ObtenerTodos();
            ViewBag.inquilino = repoInquilino.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler)
        {
			repoInmueble.CambioDisponible(alquiler.IdInmueble);
			try
            {

				repositorio.Alta(alquiler);
				TempData["Id"] = "Se creó correctamente";
                    return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.inmueble = repoInmueble.ObtenerTodos();
                ViewBag.inquilino = repoInquilino.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                //return View(alquiler);
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
			//if (TempData.ContainsKey("Mensaje"))
			//    ViewBag.Mensaje = TempData["Mensaje"];
			//if (TempData.ContainsKey("Error"))
			//    ViewBag.Error = TempData["Error"];
			return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inmueble entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Id"] = "Se eliminó correctamente";
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
			var entidad = repositorio.ObtenerPorId(id);
			ViewBag.inmueble = repoInmueble.ObtenerTodos();
			ViewBag.inquilino = repoInquilino.ObtenerTodos();
			return View(entidad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Alquiler entidad)
		{
			try
			{
				entidad.IdAlquiler = id;
				repositorio.Modificacion(entidad);
				TempData["Id"] = "";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.inmueble = repoInmueble.ObtenerTodos();
				ViewBag.inquilino = repoInquilino.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(entidad);
			}
		}
	}
}