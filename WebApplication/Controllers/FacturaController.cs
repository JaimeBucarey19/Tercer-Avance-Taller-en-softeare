using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Cliente;
using WebApplication.Modules.Factura;
using WebApplication.Modules.Lugar;

namespace WebApplication.Controllers
{
    public class FacturaController : Controller
    {
        // GET: Pedido
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        public ActionResult Factura()
        {

            List<Cliente> clientes = db.Cliente.ToList();
            ViewBag.clientes = clientes;

            List<Lugar> lugar = db.Lugars.ToList();
            ViewBag.lugar = lugar;


            return View();
        }

        public ActionResult RegistroFactura()
        {
            return View(db.Facturas.ToList());
        }
        public ActionResult Editar(int? id)
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
        public ActionResult Editar([Bind(Include = "Id,Rut,Cliente,EstadoFactura,Lugar,Giro,Email,Contacto,Fecha")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroFactura");
            }
            return View(factura);
        }
        public ActionResult Eliminar(int? id)
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
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Facturas.Find(id);
            db.Facturas.Remove(factura);
            db.SaveChanges();
            return RedirectToAction("RegistroFactura");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult GuardarFactura()
        {
            mensaje Mensajes;
            try
            {
                var Rut = JsonConvert.DeserializeObject<string>(Request.Form["Rut"]);
                var Cliente = JsonConvert.DeserializeObject<string>(Request.Form["Cliente"]);
                var EstadoFactura = JsonConvert.DeserializeObject<string>(Request.Form["EstadoFactura"]);
                var Lugar = JsonConvert.DeserializeObject<string>(Request.Form["Lugar"]);
                var Giro = JsonConvert.DeserializeObject<string>(Request.Form["Giro"]);
                var Email = JsonConvert.DeserializeObject<string>(Request.Form["Email"]);
                var Contacto = JsonConvert.DeserializeObject<string>(Request.Form["Contacto"]);
                var Fecha = JsonConvert.DeserializeObject<string>(Request.Form["Fecha"]);
                Factura factura = new Factura()
                {
                    Rut = Rut,
                    Cliente = Cliente,
                    EstadoFactura = EstadoFactura,
                    Lugar = Lugar,
                    Giro = Giro,
                    Email = Email,
                    Contacto = Contacto,
                    Fecha = Fecha,
                };
                db.Facturas.Add(factura);
                db.SaveChanges();
                var url = Url.Action("NuevoDetalleFactura", "DetalleFactura", new { id = factura.Id });

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