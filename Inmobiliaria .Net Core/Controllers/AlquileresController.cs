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
        private readonly IRepositorio<Alquiler> repositorio;
		private readonly IRepositorioInmueble repoInmueble;
        private readonly IRepositorio<Inquilino> repoInquilino;

        public AlquileresController(IRepositorio<Alquiler> repositorio, IRepositorioInmueble repoInmueble, IRepositorio<Inquilino> repoInquilino)
        {
            this.repositorio = repositorio;
            this.repoInmueble = repoInmueble;
            this.repoInquilino = repoInquilino;
        }
        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			if (TempData.ContainsKey("Alta"))
				ViewBag.Alta = TempData["Alta"];
			if (TempData.ContainsKey("Error2"))
				ViewBag.Error2 = TempData["Error2"];
			return View(lista);
        }

        public ActionResult Create()
        {
            ViewBag.inmueble = repoInmueble.ObtenerTodos();
            ViewBag.inquilino = repoInquilino.ObtenerTodos();
			int resultado = 0;
			int resultado2 = 0;

			foreach (var item in (IList<Inmueble>)ViewBag.inmueble)
			{
				if (item.Disponible.Equals("SI"))
				{
					resultado++;
				}
			}

			foreach (var item in (IList<Inquilino>)ViewBag.inquilino)
			{
				if (!item.IdInquilino.Equals(" "))
				{
					resultado2++;
				}
			}

			if (resultado > 0 && resultado2 > 0)
			{
				return View();
			}
			else
			{
				TempData["Error2"] = "No hay inmuebles o inquilinos disponibles";
				return RedirectToAction(nameof(Index));
			}
			
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler)
        {
			
			try
            {
				repoInmueble.CambioDisponible(alquiler.IdInmueble, "NO");
				if (ModelState.IsValid)
				{
					repositorio.Alta(alquiler);
					TempData["Alta"] = "Contrato de alquiler agregado correctamente";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					repoInmueble.CambioDisponible(alquiler.IdInmueble, "SI");
					ViewBag.inmueble = repoInmueble.ObtenerTodos();
					ViewBag.inquilino = repoInquilino.ObtenerTodos();
					return View();
				}
			}
            catch (Exception ex)
            {
                ViewBag.inmueble = repoInmueble.ObtenerTodos();
                ViewBag.inquilino = repoInquilino.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View();
        
            }
        }

        public ActionResult Delete(int id)
        {
            var entidad = repositorio.ObtenerPorId(id);
			return View(entidad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Alquiler entidad)
        {
			try
            {
					
				repositorio.Baja(id);
                TempData["Alta"] = "Se eliminó correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
				ViewBag.Error = "Hay pagos relacionados a este alquiler";
                ViewBag.StackTrate = ex.StackTrace;
                return View();
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
				TempData["Alta"] = "Datos modficados con exito!";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.inmueble = repoInmueble.ObtenerTodos();
				ViewBag.inquilino = repoInquilino.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
		}
	}
}