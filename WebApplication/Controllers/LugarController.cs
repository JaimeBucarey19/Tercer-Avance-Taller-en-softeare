using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Lugar;

namespace WebApplication.Controllers
{
    public class LugarController : Controller
    {
        // GET: Ciudad
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
        public ActionResult NuevoLugar()
        {
            return View();
        }
        public ActionResult RegistroLugar()
        {
            return View(db.Lugars.ToList());
        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lugar lugar = db.Lugars.Find(id);
            if (lugar == null)
            {
                return HttpNotFound();
            }
            return View(lugar);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Descripcion")] Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lugar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroLugar");
            }
            return View(lugar);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lugar lugar = db.Lugars.Find(id);
            if (lugar == null)
            {
                return HttpNotFound();
            }
            return View(lugar);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lugar lugar = db.Lugars.Find(id);
            db.Lugars.Remove(lugar);
            db.SaveChanges();
            return RedirectToAction("RegistroLugar");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GuardarLugar()
        {
            mensaje Mensajes;
            try
            {
                var Descripcion = JsonConvert.DeserializeObject<string>(Request.Form["Descripcion"]);
                Lugar lugar = new Lugar()
                {
                    Descripcion = Descripcion

                };
                db.Lugars.Add(lugar);
                db.SaveChanges();
                var url = Url.Action("NuevoLugar", "Lugar");

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