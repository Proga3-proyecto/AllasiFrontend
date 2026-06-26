namespace Progra3_Frontend.Model
{
    public class PedidoDetalleReceta
    {
        public int id { get; set; }
        public Pedido pedido { get; set; }
        public RecetaSnapshot recetaSnapshot { get; set; }
        public int cantidad { get; set; }
        public double descuentoHistorico { get; set; }
    }
}
