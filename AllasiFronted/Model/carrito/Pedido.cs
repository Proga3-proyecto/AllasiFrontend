namespace Progra3_Frontend.Model
{
    public class Pedido
    {
        public int id { get; set; }
        public Cliente cliente { get; set; }
        public DateTime? fechaPedido { get; set; }
        public DateTime? horaInicio { get; set; }
        public DateTime? horaFin { get; set; }
        public double precioTotal { get; set; }
        public double totalImpuestos { get; set; }
        public double precioDelivery { get; set; }
        public double precioFinal { get; set; }
        public EstadoPedido estado { get; set; }
        public string direccionDestino { get; set; }
        public List<PedidoDetalleProducto> detallesProductos { get; set; } = new List<PedidoDetalleProducto>();
        public List<PedidoDetalleReceta> detallesRecetas { get; set; } = new List<PedidoDetalleReceta>();
    }
}
