using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.TipoMaquina;

namespace WebApplication.Controllers
{
    public class TipoMaquinaController : Controller
    {
        // GET: TipoMaquina
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
        public ActionResult NuevoTipoMaquina()
        {
            return View();
        }

        public ActionResult RegistroTipoMaquina()
        {
            return View(db.TipoMaquinas.ToList());
        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoMaquina tipoMaquina = db.TipoMaquinas.Find(id);
            if (tipoMaquina == null)
            {
                return HttpNotFound();
            }
            return View(tipoMaquina);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Descripcion")] TipoMaquina tipoMaquina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoMaquina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroTipoMaquina");
            }
            return View(tipoMaquina);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoMaquina tipoMaquina = db.TipoMaquinas.Find(id);
            if (tipoMaquina == null)
            {
                return HttpNotFound();
            }
            return View(tipoMaquina);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoMaquina tipoMaquina = db.TipoMaquinas.Find(id);
            db.TipoMaquinas.Remove(tipoMaquina);
            db.SaveChanges();
            return RedirectToAction("RegistroTipoMaquina");
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
        public ActionResult GuardarTipoMaquina()
        {
            mensaje Mensajes;
            try
            {
                var Descripcion = JsonConvert.DeserializeObject<string>(Request.Form["Descripcion"]);
                TipoMaquina tipoMaquina = new TipoMaquina()
                {
                    Descripcion = Descripcion
                };
                db.TipoMaquinas.Add(tipoMaquina);
                db.SaveChanges();
                var url = Url.Action("NuevoMaquina", "TipoMaquina");

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
