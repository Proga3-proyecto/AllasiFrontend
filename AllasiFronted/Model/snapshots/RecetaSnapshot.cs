using System.Collections.Generic;
using System.Linq;

namespace Progra3_Frontend.Model
{
    public class RecetaSnapshot
    {
        public int? id { get; set; }
        public Receta recetaOriginal { get; set; }
        public string nombre { get; set; } = string.Empty;
        public double precioHistorico { get; set; }
        public double precioFinalHistorico { get; set; }

        public Imagen imagen { get; set; }
        public List<RecetaSnapshotElemento> elementosHistoricos { get; set; }

        public RecetaSnapshot()
        {
            this.elementosHistoricos = new List<RecetaSnapshotElemento>();
        }

        public RecetaSnapshot(Receta receta) : this()
        {
            if (receta != null)
            {
                this.recetaOriginal = receta;
                this.nombre = receta.nombre;
                this.precioHistorico = receta.precio;
                this.precioFinalHistorico = receta.precioFinal;

                if (receta.imagenes != null && receta.imagenes.Any())
                {
                    this.imagen = receta.imagenes.First();
                }

                if (receta.elementos != null)
                {
                    foreach (var elementoVivo in receta.elementos)
                    {
                        RecetaSnapshotElemento elementoHistorico = new RecetaSnapshotElemento(elementoVivo);
                        this.elementosHistoricos.Add(elementoHistorico);
                    }
                }
            }
        }
    }
}