using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.FichaEmpleado;

namespace WebApplication.Controllers
{
    public class FichaEmpleadoController : Controller
    {
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        // GET: FichaEmpleado
        private ApplicationContextDb db = new ApplicationContextDb();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NuevoFichaEmpleado()
        {
            return View();
        }
        public ActionResult RegistroFichaEmpleado()
        {
            return View(db.FichaEmpleados.ToList());
        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichaEmpleado fichaEmpleado = db.FichaEmpleados.Find(id);
            if (fichaEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(fichaEmpleado);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Rut,Nombre,Apellido,Direccion,Email,Telefono,Edad,Rol")] FichaEmpleado fichaEmpleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fichaEmpleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroFichaEmpleado");
            }
            return View(fichaEmpleado);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FichaEmpleado fichaEmpleado = db.FichaEmpleados.Find(id);
            if (fichaEmpleado == null)
            {
                return HttpNotFound();
            }
            return View(fichaEmpleado);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FichaEmpleado fichaEmpleado = db.FichaEmpleados.Find(id);
            db.FichaEmpleados.Remove(fichaEmpleado);
            db.SaveChanges();
            return RedirectToAction("RegistroFichaEmpleado");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GuardarFichaEmpleado()
        {
            mensaje Mensajes;
            try
            {
                var Nombre = JsonConvert.DeserializeObject<string>(Request.Form["Nombre"]);
                var Apellido = JsonConvert.DeserializeObject<string>(Request.Form["Apellido"]);
                var Rut = JsonConvert.DeserializeObject<string>(Request.Form["Rut"]);
                FichaEmpleado cl = db.FichaEmpleados.Where(x => x.Rut == Rut).FirstOrDefault();
                if (cl != null)
                {
                    throw new Exception("El Rut se encuentra registrado");
                }
                var Telefono = JsonConvert.DeserializeObject<string>(Request.Form["Telefono"]);
                var Email = JsonConvert.DeserializeObject<string>(Request.Form["Email"]);
                var Direccion = JsonConvert.DeserializeObject<string>(Request.Form["Direccion"]);
                var Edad = JsonConvert.DeserializeObject<string>(Request.Form["Edad"]);
                var Rol = JsonConvert.DeserializeObject<string>(Request.Form["Rol"]);
                FichaEmpleado fichaEmpleado = new FichaEmpleado()
                {
                    Nombre = Nombre,
                    Apellido = Apellido,
                    Rut = Rut,
                    Telefono = Telefono,
                    Email = Email,
                    Direccion = Direccion,
                    Edad = Edad,
                    Rol = Rol
                };
                db.FichaEmpleados.Add(fichaEmpleado);
                db.SaveChanges();
                var url = Url.Action("NuevoFichaEmpleado", "FichaEmpleado");

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