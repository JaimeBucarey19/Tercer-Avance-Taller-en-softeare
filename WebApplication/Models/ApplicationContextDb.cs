namespace WebApplication.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using WebApplication.Modules.Cliente;
    using WebApplication.Modules.FichaEmpleado;
    using WebApplication.Modules.Parametros;
    using WebApplication.Modules.Pedido;
    using WebApplication.Modules.Lugar;
    using WebApplication.Modules.Estado;
    using WebApplication.Modules.DetallePedido;
    using WebApplication.Modules.Rol;
    using WebApplication.Modules.Maquina;
    using WebApplication.Modules.TipoMaquina;
    using WebApplication.Modules.Factura;
    using WebApplication.Modules.DetalleFactura;
    using WebApplication.Modules.Presupuesto;
    using WebApplication.Modules.DetallePresupuesto;
    using WebApplication.Modules.EstadoPresupuesto;
    using WebApplication.Modules.EstadoFactura;

    public class ApplicationContextDb : DbContext
    {
        // El contexto se ha configurado para usar una cadena de conexión 'ApplicationContextDb' del archivo 
        // de configuración de la aplicación (App.config o Web.config). De forma predeterminada, 
        // esta cadena de conexión tiene como destino la base de datos 'WebApplication.Models.ApplicationContextDb' de la instancia LocalDb. 
        // 
        // Si desea tener como destino una base de datos y/o un proveedor de base de datos diferente, 
        // modifique la cadena de conexión 'ApplicationContextDb'  en el archivo de configuración de la aplicación.
        public ApplicationContextDb()
            : base("name=ApplicationContextDb")
        {
        }

        // Agregue un DbSet para cada tipo de entidad que desee incluir en el modelo. Para obtener más información 
        // sobre cómo configurar y usar un modelo Code First, vea http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public DbSet<TipoServicio> TipoServicios { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<FichaEmpleado> FichaEmpleados { get; set; }
        public DbSet<Lugar> Lugars { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<DetallePedido> DetallePedidos { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<Maquina> Maquinas { get; set; }
        public DbSet<TipoMaquina> TipoMaquinas { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }
        public DbSet<Presupuesto> Presupuestos { get; set; }
        public DbSet<DetallePresupuesto> DetallePresupuestos { get; set; } 
        public DbSet<EstadoPresupuesto> EstadoPresupuestos { get; set; }
        public DbSet<EstadoFactura> EstadoFacturas { get; set; }
    
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}