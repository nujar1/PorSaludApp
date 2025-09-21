using PorSaludApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace PorSaludApp.Controllers
{
    public class DocumentosController : Controller
    {
        private PorsaludDbContext db = new PorsaludDbContext();

    

        // GET: Documentos
        public ActionResult Index(int Id)
        {
            // Tu código existente
            var cliente = db.Clientes.Find(Id);
            if (cliente == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClienteNombre = cliente.NombreCompleto;
            ViewBag.Id = Id;

            var documentos = db.Documentos
                              .Where(d => d.ClienteId == Id && d.Activo)
                              .ToList();

            return View(documentos);
        }

        // GET: Descarga de documentos
        public ActionResult Descargar(int id)
        {
            var documento = db.Documentos.Find(id);
            if (documento == null || !documento.Activo)
            {
                return HttpNotFound();
            }

            var rutaFisica = Server.MapPath(documento.RutaArchivo);
            if (!System.IO.File.Exists(rutaFisica))
            {
                return HttpNotFound();
            }

            return File(rutaFisica, "application/octet-stream", documento.NombreArchivo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Subir(int Id, HttpPostedFileBase archivo)
        {
            try
            {
                if (archivo == null || archivo.ContentLength == 0)
                {
                    TempData["Mensaje"] = "No se ha seleccionado ningún archivo.";
                    return RedirectToAction("Index", new { Id });
                }

                // Validar tipo de archivo
                var extensionesPermitidas = new[] { ".pdf" };
                var extension = Path.GetExtension(archivo.FileName).ToLower();

                if (!extensionesPermitidas.Contains(extension))
                {
                    TempData["Mensaje"] = "Tipo de archivo no permitido. Solo archivos PDF permitidos.";
                    return RedirectToAction("Index", new { Id });
                }

                // Crear directorio si no existe
                var rutaDirectorio = Server.MapPath("~/Documents/");
                if (!Directory.Exists(rutaDirectorio))
                {
                    Directory.CreateDirectory(rutaDirectorio);
                }

                // Generar nombre único para el archivo
                var nombreArchivoUnico = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(rutaDirectorio, nombreArchivoUnico);

                // Guardar archivo
                archivo.SaveAs(rutaCompleta);

                // Guardar en base de datos
                var documento = new Documento
                {
                    ClienteId = Id,
                    NombreArchivo = archivo.FileName,
                    RutaArchivo = "/Documents/" + nombreArchivoUnico,
                    TipoArchivo = extension,
                    FechaCarga = DateTime.Now,
                    Activo = true
                };

                db.Documentos.Add(documento);
                db.SaveChanges();

                TempData["Mensaje"] = "Archivo subido correctamente.";
                return RedirectToAction("Index", new { Id });
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al subir el archivo: " + ex.Message;
                return RedirectToAction("Index", new { Id });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var documento = db.Documentos.Find(id);
                if (documento != null)
                {
                    // Eliminar archivo físico
                    var rutaFisica = Server.MapPath(documento.RutaArchivo);
                    if (System.IO.File.Exists(rutaFisica))
                    {
                        System.IO.File.Delete(rutaFisica);
                    }

                    // Eliminar de la base de datos
                    db.Documentos.Remove(documento);
                    db.SaveChanges();

                    TempData["Mensaje"] = "Documento eliminado correctamente.";
                    return RedirectToAction("Index", new { Id = documento.ClienteId });
                }

                TempData["Mensaje"] = "Documento no encontrado.";
                return RedirectToAction("Index", "Clientes");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar el documento: " + ex.Message;
                return RedirectToAction("Index", "Clientes");
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