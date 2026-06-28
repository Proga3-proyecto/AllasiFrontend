namespace AllasiFrontend.Model.carrito
{
    public class ActualizarCantidadRequest
    {
        public int cantidad { get; set; }

        public ActualizarCantidadRequest(int cantidad)
        {
            this.cantidad = cantidad;
        }
    }
}
