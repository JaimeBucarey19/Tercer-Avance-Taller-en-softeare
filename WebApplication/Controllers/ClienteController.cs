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

namespace WebApplication.Controllers
{
    public class ClienteController : Controller
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
        public ActionResult NuevoCliente()
        {
            return View();
        }
        public ActionResult RegistroCliente()
        {
            return View(db.Cliente.ToList());
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Rut,Nombre,Apellido,Direccion,Email,Telefono,Giro,PersonaContacto")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroCliente");
            }
            return View(cliente);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("RegistroCliente");
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
        public ActionResult GuardarCliente()
        {
            mensaje Mensajes;
            try
            {
                var Run = JsonConvert.DeserializeObject<string>(Request.Form["Run"]);
                var Nombres = JsonConvert.DeserializeObject<string>(Request.Form["Nombres"]);
                var Apellido = JsonConvert.DeserializeObject<string>(Request.Form["Apellido"]);
                Cliente cl = db.Cliente.Where(x => x.Rut == Run).FirstOrDefault();
                if (cl != null)
                {
                    throw new Exception("El Rut se encuentra registrado");
                }
                var Direccion = JsonConvert.DeserializeObject<string>(Request.Form["Direccion"]);
                var Telefono = JsonConvert.DeserializeObject<string>(Request.Form["Telefono"]);
                var Giro = JsonConvert.DeserializeObject<string>(Request.Form["Giro"]);
                var Email = JsonConvert.DeserializeObject<string>(Request.Form["Email"]);
                var PersonaContacto = JsonConvert.DeserializeObject<string>(Request.Form["PersonaContacto"]);
                Cliente cliente = new Cliente()
                {
                    Nombre = Nombres,
                    Apellido= Apellido,
                    Rut = Run,
                    Direccion = Direccion,
                    Telefono = Telefono,
                    Giro = Giro,
                    Email = Email,
                    PersonaContacto = PersonaContacto
                };
                db.Cliente.Add(cliente);
                db.SaveChanges();
                var url = Url.Action("NuevoCliente", "Cliente");

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