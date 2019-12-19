using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Parametros;

namespace WebApplication.Controllers
{
    public class TipoServicioController : Controller
    {
        // GET: TipoServicio
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }

        // GET: TipoServicios
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NuevoTipoServicio()
        {
            return View();
        }

        public ActionResult RegistroTipoServicio()
        {
            return View(db.TipoServicios.ToList());
        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicio tipoServicio = db.TipoServicios.Find(id);
            if (tipoServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoServicio);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Descripcion")] TipoServicio tipoServicio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoServicio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroTipoServicio");
            }
            return View(tipoServicio);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServicio tipoServicio = db.TipoServicios.Find(id);
            if (tipoServicio == null)
            {
                return HttpNotFound();
            }
            return View(tipoServicio);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoServicio tipoServicio = db.TipoServicios.Find(id);
            db.TipoServicios.Remove(tipoServicio);
            db.SaveChanges();
            return RedirectToAction("RegistroTipoServicio");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: TipoServicios/Details/5
        public ActionResult GuardarTipoServicio()
        {
            mensaje Mensajes;
            try
            {
                var Descripcion = JsonConvert.DeserializeObject<string>(Request.Form["Descripcion"]);
                TipoServicio tipoServicio = new TipoServicio()
                {
                    Descripcion = Descripcion
                };
                db.TipoServicios.Add(tipoServicio);
                db.SaveChanges();
                var url = Url.Action("NuevoTipoServicio", "TipoServicio");

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
                    isRedirect = false,
                    Mensajes
                }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
