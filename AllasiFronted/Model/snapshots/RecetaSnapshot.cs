namespace Progra3_Frontend.Model
{
    public class RecetaSnapshot
    {
        public int id { get; set; }
        public Receta recetaOriginal { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public string instrucciones { get; set; } = string.Empty;
        public double precioHistorico { get; set; }
        public double precioFinalHistorico { get; set; }
        public List<Imagen> imagenesHistoricas { get; set; } = new List<Imagen>();
        public List<RecetaSnapshotElemento> elementosHistoricos { get; set; } = new List<RecetaSnapshotElemento>();
        public RecetaSnapshot()
        {
        }
        public RecetaSnapshot(Receta receta) : this()
        {
            if (receta != null)
            {
                this.recetaOriginal = receta;
                this.nombre = receta.nombre;
                this.descripcion = receta.descripcion;
                this.instrucciones = receta.instrucciones;
                this.precioHistorico = receta.precio;
                this.precioFinalHistorico = receta.precioFinal;

                if (receta.imagenes != null)
                {
                    this.imagenesHistoricas.AddRange(receta.imagenes);
                }

                if (receta.elementos != null)
                {
                    foreach (var elementoVivo in receta.elementos)
                    {
                        RecetaSnapshotElemento elementoHistorico = new RecetaSnapshotElemento(elementoVivo, this);
                        this.elementosHistoricos.Add(elementoHistorico);
                    }
                }
            }
        }
    }
}