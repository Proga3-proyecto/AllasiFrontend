namespace Progra3_Frontend.Model
{
    public class RecetaSnapshotElemento
    {
        public ProductoSnapshot productoSnapshot { get; set; }
        public double cantidad { get; set; }

        public RecetaSnapshotElemento()
        {
        }

        public RecetaSnapshotElemento(ElementoReceta elementoVivo)
        {
            if (elementoVivo != null)
            {
                this.cantidad = elementoVivo.cantidad;

                if (elementoVivo.producto != null)
                {
                    this.productoSnapshot = new ProductoSnapshot(elementoVivo.producto);
                }
            }
        }
    }
}