namespace Progra3_Frontend.Model
{
    public class PedidoDetalleProducto
    {
        public int id { get; set; }
        public Pedido pedido { get; set; }
        public ProductoSnapshot productoSnapshot { get; set; }
        public int cantidad { get; set; }
    }
}
