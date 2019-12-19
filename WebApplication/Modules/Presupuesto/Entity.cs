using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.Presupuesto
{
    [Table("public.Presupuesto")]
    public class Presupuesto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EstadoPresupuesto { get; set; }
        public string Rut { get; set; }
        public string Cliente { get; set; }
        public string Lugar { get; set; }
        public string Email { get; set; }
        public string DiasAviles { get; set; }
        public string Fecha { get; set; }
    }
}