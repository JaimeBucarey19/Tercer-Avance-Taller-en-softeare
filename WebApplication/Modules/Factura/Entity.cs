using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.Factura
{
    [Table("public.Factura")]
    public class Factura
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Rut { get; set; }
        public string Cliente { get; set; }
        public string EstadoFactura { get; set; }
        public string Lugar { get; set; }
        public string Giro { get; set; }
        public string Email { get; set; }
        public string Contacto { get; set; }
        public string Fecha { get; set; }

    }
}