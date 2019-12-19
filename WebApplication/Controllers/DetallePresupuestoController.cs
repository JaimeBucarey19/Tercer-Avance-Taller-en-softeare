using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.DetallePresupuesto;
using WebApplication.Modules.Presupuesto;

namespace WebApplication.Controllers
{
    public class DetallePresupuestoController : Controller
    {
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        public ActionResult NuevoDetallePresupuesto(int Id)
        {

            ViewBag.PresupuestoId = Id;
            Presupuesto presupuesto = db.Presupuestos.Find(Id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            return View(presupuesto);

        }
        public ActionResult RegistroDetallePresupuesto()
        {
            return View(db.DetallePresupuestos.ToList());
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePresupuesto detallePresupuesto = db.DetallePresupuestos.Find(id);
            if (detallePresupuesto == null)
            {
                return HttpNotFound();
            }
            return View(detallePresupuesto);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,PresupuestoId,Precio,Cantidad,FormaDePago,Iva,Descuento,Total,Descripcion")] DetallePresupuesto detallePresupuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePresupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroDetallePresupuesto");
            }
            return View(detallePresupuesto);
        }

        public ActionResult EditarPr(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presupuesto presupuesto = db.Presupuestos.Find(id);
            if (presupuesto == null)
            {
                return HttpNotFound();
            }
            return View(presupuesto);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditarPr([Bind(Include = "Id,EstadoPresupuesto,Rut,Cliente,Lugar,Email,DiasAviles,Fecha")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NuevoDetallePresupuesto", new { id = presupuesto.Id });
            }
            return View(presupuesto);
        }

        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePresupuesto detallePresupuesto = db.DetallePresupuestos.Find(id);
            if (detallePresupuesto == null)
            {
                return HttpNotFound();
            }
            return View(detallePresupuesto);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallePresupuesto detallePresupuesto = db.DetallePresupuestos.Find(id);
            db.DetallePresupuestos.Remove(detallePresupuesto);
            db.SaveChanges();
            return RedirectToAction("RegistroDetallePresupuesto");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]

        public ActionResult SaveOrder()
        {
            mensaje Mensajes;
            try
            {
                //string detalles_json = JsonConvert.SerializeObject();
                var Detalles = JsonConvert.DeserializeObject<List<DetallePresupuesto>>(Request.Form["DetallePresupuesto"]);

                foreach (var item in Detalles)
                {
                    DetallePresupuesto O = new DetallePresupuesto();
                    O.PresupuestoId = item.PresupuestoId;
                    O.Precio = item.Precio;
                    O.Cantidad = item.Cantidad;
                    O.FormaDePago = item.FormaDePago;
                    O.Iva = item.Iva;
                    O.Descuento = item.Descuento;
                    O.Total = item.Total;
                    O.Descripcion = item.Descripcion;


                    db.DetallePresupuestos.Add(O);
                }
                db.SaveChanges();
                var url = Url.Action("Presupuesto", "Presupuesto");
                Mensajes = new mensaje()
                {
                    msg = "Guardado Exitosamente",
                    tipo = "success",
                    titulo = "Éxito"
                };
                return Json(new
                {
                    redirectUrl = url,
                    isRedirect = true,
                    Mensajes

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Mensajes = new mensaje()
                {
                    msg = ex.Message,
                    tipo = "error",
                    titulo = "Opps! a ocurrido un problema"
                };
                return Json(new
                {
                    isRedirect = true,

                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}