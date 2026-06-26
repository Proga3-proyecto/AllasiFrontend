namespace Progra3_Frontend.Model
{
    public class DetalleReceta
    {

        public int id { get; set;  }
        public Receta receta { get; set; }
        public Cliente clienteCarrito { get; set; }
        public int cantidad { get; set; }
        public double descuentoTotal { get; set; }
        public double montoTotal { get; set; }
    }
}
