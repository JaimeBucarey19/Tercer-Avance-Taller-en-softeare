using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.DetallePresupuesto
{
    [Table("public.DetallePresupuesto")]
    public class DetallePresupuesto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PresupuestoId { get; set; }
        public string Precio { get; set; }
        public string Cantidad { get; set; }
        public string FormaDePago { get; set; }
        public string Iva { get; set; }
        public string Descuento { get; set; }
        public string Total { get; set; }
        public string Descripcion { get; set; }
    }
}