using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.DetallePedido
{
    [Table("public.DetallePedido")]
    public class DetallePedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Pedido_Id { get; set; }
        public string PuntoSalida { get; set; }
        public string PuntoLlegada { get; set; }
        public string Kilometros { get; set; }
        public string PrecioKilometro { get; set; }
        public string CabinasDePeaje { get; set;}
        public string Descuento { get; set; }
        public string Iva { get; set; }
        public string Descripcion { get; set; }
        public string ValorTotal { get; set; }

    }
}