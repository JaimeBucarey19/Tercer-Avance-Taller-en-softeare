using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.DetalleFactura;
using WebApplication.Modules.Factura;

namespace WebApplication.Controllers
{
    public class DetalleFacturaController : Controller
    {
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        public ActionResult NuevoDetalleFactura(int Id)
        {


            ViewBag.FacturaId = Id;
            Factura factura = db.Facturas.Find(Id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);



        }
        public ActionResult RegistroDetalleFactura()
        {
            return View(db.DetalleFacturas.ToList());
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleFactura detalleFactura = db.DetalleFacturas.Find(id);
            if (detalleFactura == null)
            {
                return HttpNotFound();
            }
            return View(detalleFactura);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,FacturaId, Precio, Cantidad, MontoNeto, Iva, Descuento, ImpuestoAdicional,Descripcion, Total")] DetalleFactura detalleFactura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleFactura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroDetalleFactura");
            }
            return View(detalleFactura);
        }

        public ActionResult EditarF(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult EditarF([Bind(Include = "Id,Rut,Cliente,EstadoFactura,Lugar,Giro,Email,Contacto,Fecha")] Factura Factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NuevoDetalleFactura", new { id = Factura.Id });
            }
            return View(Factura);
        }

        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleFactura detalleFactura = db.DetalleFacturas.Find(id);
            if (detalleFactura == null)
            {
                return HttpNotFound();
            }
            return View(detalleFactura);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleFactura detalleFactura = db.DetalleFacturas.Find(id);
            db.DetalleFacturas.Remove(detalleFactura);
            db.SaveChanges();
            return RedirectToAction("RegistroDetalleFactura");
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
                var Detalles = JsonConvert.DeserializeObject<List<DetalleFactura>>(Request.Form["DetalleFactura"]);

                foreach (var item in Detalles)
                {
                    DetalleFactura O = new DetalleFactura();
                    O.FacturaId = item.FacturaId;
                    O.Precio = item.Precio;
                    O.Cantidad = item.Cantidad;
                    O.MontoNeto = item.MontoNeto;
                    O.Iva = item.Iva;
                    O.Descuento = item.Descuento;
                    O.ImpuestoAdicional = item.ImpuestoAdicional;
                    O.Total = item.Total;
                    O.Descripcion = item.Descripcion;


                    db.DetalleFacturas.Add(O);
                }
                db.SaveChanges();
                var url = Url.Action("Factura", "Factura");
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