using PorSaludApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PorSaludApp.Controllers
{
    public class ClientesController : Controller
    {
        private PorsaludDbContext db = new PorsaludDbContext();


        // GET: Obtener todos los clientes
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            int totalRecords = db.Clientes.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var clientes = db.Clientes
                            .OrderBy(c => c.ClienteId)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;

            return View(clientes);
        }

      
        public ActionResult CrearCliente()
        {
            return View();
        }

        // POST: Crear un nuevo cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CrearCliente(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cliente.FechaCreacion = DateTime.Now;
                    db.Clientes.Add(cliente);
                    await db.SaveChangesAsync();

                    TempData["SweetAlertMessage"] = "Cliente creado exitosamente";
                    TempData["SweetAlertType"] = "success";
                    TempData["SweetAlertTitle"] = "¡Éxito!";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["SweetAlertMessage"] = "Error al crear cliente: " + ex.Message;
                TempData["SweetAlertType"] = "error";
                TempData["SweetAlertTitle"] = "Error";
                ModelState.AddModelError("", ex.Message);
            }
            return View(cliente);
        }

        // GET: Editar Clientes 
        public async Task<ActionResult> EditarCliente(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                Cliente cliente = await db.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return HttpNotFound();
                }
                ViewBag.EditarCliente = cliente;

                return View(cliente);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al cargar cliente: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Editar Cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarCliente(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(cliente).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar cliente: " + ex.Message);
            }
            return View(cliente);
        }

        // POST: Eliminar cliente (Inactivar)
        // Soft delete - marcar como inactivo
        // Lo hice asi porque como buena practica no se deben de eliminar los registros
        //solo se deben de inhabilitar 
        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                Cliente cliente = await db.Clientes.FindAsync(id);
                if (cliente == null)
                {
                    return Json(new { success = false, message = "Cliente no encontrado" });
                }

            
                cliente.Estado = false;
                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Cliente inactivado correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}

