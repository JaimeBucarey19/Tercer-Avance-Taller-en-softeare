using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.Estado;

namespace WebApplication.Controllers
{
    public class EstadoController : Controller
    {
        // GET: Estado
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
        public ActionResult NuevoEstado()
        {
            return View();
        }
        public ActionResult GuardarEstado()
        {
            mensaje Mensajes;
            try
            {
                var Descripcion = JsonConvert.DeserializeObject<string>(Request.Form["Descripcion"]);
                Estado estado = new Estado()
                {
                    Descripcion = Descripcion

                };
                db.Estados.Add(estado);
                db.SaveChanges();
                var url = Url.Action("NuevoEstado", "Estado");

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