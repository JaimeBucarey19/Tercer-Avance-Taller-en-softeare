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
using WebApplication.Modules.Estado;
using WebApplication.Modules.FichaEmpleado;
using WebApplication.Modules.Lugar;
using WebApplication.Modules.Maquina;
using WebApplication.Modules.Parametros;
using WebApplication.Modules.Pedido;

namespace WebApplication.Controllers
{
    public class PedidoController : Controller
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
        public ActionResult Pedido()
        {

            List<Cliente> clientes = db.Cliente.ToList();
            ViewBag.clientes = clientes;

            List<TipoServicio> tipoServicio = db.TipoServicios.ToList();
            ViewBag.tipoServicio = tipoServicio;

            List<Lugar> lugar = db.Lugars.ToList();
            ViewBag.lugar = lugar;

            List<Maquina> maquina = db.Maquinas.ToList();
            ViewBag.maquina = maquina;

            return View();
        }

        public ActionResult RegistroPedido()
        {
            return View(db.Pedidos.ToList());
        }
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Cliente,TipoServicio,Lugar,Estado,Maquina,Fecha")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroPedido");
            }
            return View(pedido);
        }
        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidos.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedidos.Find(id);
            db.Pedidos.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("RegistroPedido");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult GuardarPedido()
        {
            mensaje Mensajes;
            try
            {
                var Cliente = JsonConvert.DeserializeObject<string>(Request.Form["Cliente"]);
                var TipoServicio = JsonConvert.DeserializeObject<string>(Request.Form["TipoServicio"]);
                var Lugar = JsonConvert.DeserializeObject<string>(Request.Form["Lugar"]);
                var Estado = JsonConvert.DeserializeObject<string>(Request.Form["Estado"]);
                var Maquina = JsonConvert.DeserializeObject<string>(Request.Form["Maquina"]);
                var Fecha = JsonConvert.DeserializeObject<string>(Request.Form["Fecha"]);
                Pedido pedido = new Pedido()
                {
                    Cliente = Cliente,
                    TipoServicio = TipoServicio,
                    Lugar = Lugar,
                    Estado = Estado,
                    Maquina = Maquina,
                    Fecha = Fecha,
                };
                db.Pedidos.Add(pedido);
                db.SaveChanges();
                var url = Url.Action("NuevoDetallePedido", "DetallePedido", new { id = pedido.Id });

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