using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class InmueblesController : Controller
    {
        private readonly IRepositorioInmueble repositorio;
        private readonly IRepositorioPropietario repoPropietario;

        public InmueblesController(IRepositorioInmueble repositorio, IRepositorioPropietario repoPropietario)
        {
            this.repositorio = repositorio;
            this.repoPropietario = repoPropietario;
        }

        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            //if (TempData.ContainsKey("Id"))
            //    ViewBag.Id = TempData["Id"];
            //if (TempData.ContainsKey("Mensaje"))
            //    ViewBag.Mensaje = TempData["Mensaje"];
			return View(lista);
        }
        
        public ActionResult Create()
		{
			ViewBag.Propietarios = repoPropietario.ObtenerTodos();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Inmueble entidad)
		{
            try
            {
                if (ModelState.IsValid)
                {
                    repositorio.Alta(entidad);
                    TempData["Id"] = entidad.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Propietarios = repoPropietario.ObtenerTodos();
                    return View(entidad);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
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
			var entidad = repositorio.ObtenerPorId(id);
			ViewBag.Propietarios = repoPropietario.ObtenerTodos();
			return View(entidad);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Inmueble entidad)
		{
			try
			{
				entidad.IdInmueble = id;
				repositorio.Modificacion(entidad);
				TempData["Id"] = "";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Propietarios = repoPropietario.ObtenerTodos();
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View(entidad);
			}
		}
	}
}