    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;
using WebApplication.Modules.DetallePedido;
using WebApplication.Modules.Pedido;

namespace WebApplication.Controllers
{
    public class DetallePedidoController : Controller
    {
        private ApplicationContextDb db = new ApplicationContextDb();
        public class mensaje
        {
            public string msg { get; set; }
            public string tipo { get; set; }
            public string titulo { get; set; }
        }
        public ActionResult NuevoDetallePedido(int Id)
        {
           

            ViewBag.PedidoId = Id;
            Pedido Pedido = db.Pedidos.Find(Id);
            if (Pedido == null)
            {
                return HttpNotFound();
            }
            return View(Pedido);

        }
        public ActionResult RegistroDetallePedido()
        {
            return View(db.DetallePedidos.ToList());
        }

        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            if (detallePedido == null)
            {
                return HttpNotFound();
            }
            return View(detallePedido);
        }

        // POST: TipoServicios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,PuntoSalida, PuntoLlegada, Kilometros, PrecioKilometro, CabinasDePeaje, Descuento, Iva, Descripcion, ValorTotal")] DetallePedido detallePedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detallePedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RegistroDetallePedido");
            }
            return View(detallePedido);
        }

        public ActionResult EditarP(int? id)
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
        public ActionResult EditarP([Bind(Include = "Id,Cliente,TipoServicio,Lugar,Estado,Maquina,Fecha")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NuevoDetallePedido", new { id = pedido.Id });
            }
            return View(pedido);
        }

        public ActionResult Eliminar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            if (detallePedido == null)
            {
                return HttpNotFound();
            }
            return View(detallePedido);
        }
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetallePedido detallePedido = db.DetallePedidos.Find(id);
            db.DetallePedidos.Remove(detallePedido);
            db.SaveChanges();
            return RedirectToAction("RegistroDetallePedido");
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
        
        public ActionResult Prueba()
        {
            List<Pedido> detallePedido = db.Pedidos.ToList();
            return View(detallePedido);
        }
        public ActionResult SaveOrder()
        {
            mensaje Mensajes;
            try
            {
                //string detalles_json = JsonConvert.SerializeObject();
                var Detalles = JsonConvert.DeserializeObject<List<DetallePedido>>(Request.Form["DetallePedido"]);

                foreach (var item in Detalles)
                {
                    DetallePedido O = new DetallePedido();
                    O.PuntoSalida = item.PuntoSalida;
                    O.Pedido_Id = item.Pedido_Id;
                    O.PuntoLlegada = item.PuntoLlegada;
                    O.Kilometros = item.Kilometros;
                    O.PrecioKilometro = item.PrecioKilometro;
                    O.CabinasDePeaje = item.CabinasDePeaje;
                    O.Descuento = item.Descuento;
                    O.Iva = item.Iva;
                    O.Descripcion = item.Descripcion;
                    O.ValorTotal = item.ValorTotal;

                    db.DetallePedidos.Add(O);
                }
                db.SaveChanges();
                var url = Url.Action("Pedido", "Pedido");
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
