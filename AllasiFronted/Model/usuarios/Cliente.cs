namespace Progra3_Frontend.Model
{
    public class Cliente : Usuario
    {
        public string telefono { get; set; }
        public DateTime? fechaNacimiento { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Pedido pedidoActivo { get; set; }
        public List<ClienteDireccion> direcciones { get; set; } = new List<ClienteDireccion>();
        public List<DetalleProducto> carritoProductos { get; set; } = new List<DetalleProducto>();
        public List<DetalleReceta> carritoRecetas { get; set; } = new List<DetalleReceta>();
        public Cliente()
        {
            TipoUsuario = "Cliente";
        }
    }
}