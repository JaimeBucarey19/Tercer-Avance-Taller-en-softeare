using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.Maquina
{
    [Table("public.Maquina")]
    public class Maquina
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Patente { get; set; }
        public string Nombre { get; set; }
        public string Duenio { get; set; }
        public string TipoMaquina { get; set; }
        public string Fecha { get; set; }
      

    }
}