using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication.Modules.Pedido
{
    [Table("public.Pedido")]
    public class Pedido
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string TipoServicio { get; set; }
        public string Lugar { get; set; }
        public string Estado { get; set; }
        public string Maquina { get; set; }
        public string Fecha { get; set; } 
      
    }
}