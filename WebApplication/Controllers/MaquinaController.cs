using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Maquina;

namespace WebApplication.Controllers
{
    public class MaquinaController : Controller
    {
        // GET: Cliente
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
        public ActionResult NuevoMaquina()
        {
            return View();
        }
        public ActionResult RegistroMaquina()
        {
            return View(db.Maquinas.ToList());
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquina maquina = db.Maquinas.Find(id);
            if (maquina == null)
            {
                return HttpNotFound();
            }
            return View(maquina);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Patente,Nombre,Duenio,TipoMaquina,Seguro,Fecha")] Maquina maquina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maquina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroMaquina");
            }
            return View(maquina);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maquina maquina = db.Maquinas.Find(id);
            if (maquina == null)
            {
                return HttpNotFound();
            }
            return View(maquina);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Maquina maquina = db.Maquinas.Find(id);
            db.Maquinas.Remove(maquina);
            db.SaveChanges();
            return RedirectToAction("RegistroMaquina");
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
        public ActionResult GuardarMaquina()
        {
            mensaje Mensajes;
            try
            {
                var Patente = JsonConvert.DeserializeObject<string>(Request.Form["Patente"]);
                var Nombre = JsonConvert.DeserializeObject<string>(Request.Form["Nombre"]);
                var Duenio = JsonConvert.DeserializeObject<string>(Request.Form["Duenio"]);
                var TipoMaquina = JsonConvert.DeserializeObject<string>(Request.Form["TipoMaquina"]);
                var Fecha = JsonConvert.DeserializeObject<string>(Request.Form["Fecha"]);
                Maquina maquina = new Maquina()
                {
                    Patente = Patente,
                    Nombre = Nombre,
                    Duenio = Duenio,
                    TipoMaquina = TipoMaquina,
                    Fecha = Fecha
                };
                db.Maquinas.Add(maquina);
                db.SaveChanges();
                var url = Url.Action("NuevoMaquina", "Maquina");

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