using System.Collections.Generic;
using System.Linq;

namespace Progra3_Frontend.Model
{
    public class ProductoSnapshot
    {
        public int? id { get; set; }
        public Producto productoOriginal { get; set; }
        public string nombre { get; set; } = string.Empty;
        public double precioVenta { get; set; }
        public double precioFinalVenta { get; set; }
        public double descuentoApplied { get; set; }
        public double volumenLitros { get; set; }
        public double porcentajeAlcohol { get; set; }
        public string nombreMarca { get; set; } = string.Empty;

        public string nombreImpuesto { get; set; } = string.Empty;
        public double porcentajeImpuesto { get; set; }
        public TipoImpuesto tipoImpuesto { get; set; }

        public int? porcentajePrecioAlcoholHistorico { get; set; }
        public double? valorImpuestoAlcoholHistorico { get; set; }

        public Imagen imagen { get; set; }
        public List<string> categoriasHistoricas { get; set; }

        public ProductoSnapshot()
        {
        }

        public ProductoSnapshot(Producto producto) : this()
        {
            if (producto != null)
            {
                this.productoOriginal = producto;
                this.nombre = producto.nombre;
                this.precioVenta = producto.precio;
                this.precioFinalVenta = producto.precioFinal;
                this.descuentoApplied = producto.descuento;
                this.volumenLitros = producto.volumenLitros;
                this.porcentajeAlcohol = producto.porcentajeAlcohol;

                if (producto.marca != null)
                {
                    this.nombreMarca = producto.marca.nombre;
                }

                if (producto.impuestoBase != null)
                {
                    this.nombreImpuesto = producto.impuestoBase.nombre;
                    this.porcentajeImpuesto = producto.impuestoBase.porcentaje;
                    this.tipoImpuesto = producto.impuestoBase.tipo;
                }

                if (producto.impuestoAlcohol != null)
                {
                    this.valorImpuestoAlcoholHistorico = producto.impuestoAlcohol.valor;
                }
                else
                {
                    this.porcentajePrecioAlcoholHistorico = 0;
                    this.valorImpuestoAlcoholHistorico = 0.0;
                }

                if (producto.categorias != null)
                {
                    this.categoriasHistoricas = producto.categorias.Select(c => c.nombre).ToList();
                }

                if (producto.imagenes != null && producto.imagenes.Any())
                {
                    this.imagen = producto.imagenes.First();
                }
            }
        }
    }
}