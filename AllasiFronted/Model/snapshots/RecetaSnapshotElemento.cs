namespace Progra3_Frontend.Model
{
    public class RecetaSnapshotElemento
    {
        public RecetaSnapshot recetaSnapshot { get; set; }
        public ProductoSnapshot productoSnapshot { get; set; }
        public double cantidad { get; set; }

        public RecetaSnapshotElemento()
        {
        }

        public RecetaSnapshotElemento(ElementoReceta elementoVivo, RecetaSnapshot recetaPadre)
        {
            if (elementoVivo != null)
            {
                this.recetaSnapshot = recetaPadre;
                this.cantidad = elementoVivo.cantidad;

                if (elementoVivo.producto != null)
                {
                    this.productoSnapshot = new ProductoSnapshot(elementoVivo.producto);
                }
            }
        }
    }
}