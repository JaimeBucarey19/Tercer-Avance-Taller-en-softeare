using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Lugar;
using WebApplication.Modules.Cliente;
using System.Net;
using WebApplication.Modules.Presupuesto;
using System.Data.Entity;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class PresupuestoController : Controller
    {
        // GET: Pedido
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Presupuesto()
        {

            List<Cliente> clientes = db.Cliente.ToList();
            ViewBag.clientes = clientes;

            List<Lugar> lugar = db.Lugars.ToList();
            ViewBag.lugar = lugar;


            return View();
        }

        public ActionResult RegistroPresupuesto()
        {
            return View(db.Presupuestos.ToList());
        }
        public ActionResult Editar(int? id)
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
        public ActionResult Editar([Bind(Include = "Id,EstadoPresupuesto,Rut,Cliente,Lugar,Email,DiasAviles,Fecha")] Presupuesto presupuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presupuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroPresupuesto");
            }
            return View(presupuesto);
        }
        public ActionResult Eliminar(int? id)
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
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presupuesto presupuesto = db.Presupuestos.Find(id);
            db.Presupuestos.Remove(presupuesto);
            db.SaveChanges();
            return RedirectToAction("RegistroPresupuesto");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult GuardarPresupuesto()
        {
            mensaje Mensajes;
            try
            {
                var Rut = JsonConvert.DeserializeObject<string>(Request.Form["Rut"]);
                var Cliente = JsonConvert.DeserializeObject<string>(Request.Form["Cliente"]);        
                var EstadoPresupuesto = JsonConvert.DeserializeObject<string>(Request.Form["EstadoPresupuesto"]);
                var Lugar = JsonConvert.DeserializeObject<string>(Request.Form["Lugar"]);
                var Email = JsonConvert.DeserializeObject<string>(Request.Form["Email"]);
                var DiasAviles = JsonConvert.DeserializeObject<string>(Request.Form["DiasAviles"]);
                var Fecha = JsonConvert.DeserializeObject<string>(Request.Form["Fecha"]);
                Presupuesto p = new Presupuesto()
                {
                    Rut = Rut,
                    Cliente = Cliente,
                    EstadoPresupuesto = EstadoPresupuesto,
                    Lugar = Lugar,
                    Email = Email,
                    DiasAviles = DiasAviles,
                    Fecha = Fecha,

                };
                db.Presupuestos.Add(p);
                db.SaveChanges();

                var url = Url.Action("NuevoDetallePresupuesto", "DetallePresupuesto", new { id = p.Id });

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