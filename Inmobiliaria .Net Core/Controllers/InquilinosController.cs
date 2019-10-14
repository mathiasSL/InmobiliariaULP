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
			if (TempData.ContainsKey("Alta"))
				ViewBag.Alta = TempData["Alta"];
			return View(lista);
        }
   
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
			ViewBag.inquilinos = repositorio.ObtenerTodos();

			foreach (var item in (IList<Inquilino>)ViewBag.inquilinos)
			{
				if (item.Dni == inquilino.Dni)
				{
					ViewBag.Error2 = "Error: Ya existe un inquilino con ese DNI";
					return View();
				}
			}

			try
			{
				if (ModelState.IsValid)
				{
					repositorio.Alta(inquilino);
					TempData["Alta"] = "Inquilino agregado exitosamente!";
					return RedirectToAction(nameof(Index));
				}
				else
					return View();	
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
				TempData["Alta"] = "Datos modificados con exito!";
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
				TempData["Alta"] = "Inquilino eliminado";
				return RedirectToAction(nameof(Index));
			}
            catch (Exception ex)
            {
                ViewBag.Error = "Inquilino con contrato vigente";
                ViewBag.StackTrate = ex.StackTrace;
				return View();
			}
        }
    }
}