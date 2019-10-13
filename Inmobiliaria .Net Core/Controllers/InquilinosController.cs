using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class InquilinosController : Controller
    {
		private readonly IRepositorio<Inquilino> repositorio;

		public InquilinosController(IRepositorio<Inquilino> repositorio)
		{
			this.repositorio = repositorio;
		}

        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			return View(lista);
        }

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

       
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
			try
			{
				repositorio.Alta(inquilino);
				TempData["Id"] = " ";
				return RedirectToAction(nameof(Index));
			}
			catch(Exception ex)
			{
				ViewBag.Error = ex.Message;
				ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
					
        }

       
        public ActionResult Edit(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino p = null;
            try
            {
                p = repositorio.ObtenerPorId(id);
                p.Nombre = collection["Nombre"];
                p.Apellido = collection["Apellido"];
                p.Dni = collection["Dni"];
                p.Direccion = collection["Direccion"];
                p.Telefono = collection["Telefono"];
                repositorio.Modificacion(p);
                TempData["Id"] = "";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(p);
            }
        }
       
        public ActionResult Delete(int id)
        {
            var p = repositorio.ObtenerPorId(id);
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino entidad)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Id"] = "";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "";
                ViewBag.StackTrate = ex.StackTrace;
                return View(entidad);
            }
        }
    }
}