using System.Text.Json.Serialization;

namespace Progra3_Frontend.Model
{
    public class Producto
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public double precio { get; set; }
        public double precioFinal { get; set; }
        public int stock { get; set; }
        public double descuento { get; set; }
        public double volumenLitros { get; set; }
        public double porcentajeAlcohol { get; set; }
        public string descripcion { get; set; }
        public Marca marca { get; set; } = new();
        [System.Text.Json.Serialization.JsonIgnore]
        public Impuesto impuestoBase { get; set; } = new();
        [System.Text.Json.Serialization.JsonIgnore]
        public AlcoholImpuesto impuestoAlcohol { get; set; } = new();
        public List<Categoria> categorias { get; set; } = new();
        public List<Imagen> imagenes { get; set; } = new();

        // Atributos extras

        //public bool favorito { get; set; } = false;
        [System.Text.Json.Serialization.JsonIgnore]
        public string ImagenUrl
        {
            get
            {
                if (imagenes == null) return "";
                if (imagenes.Count == 0) return "";
                return imagenes[0].url;
            }
        }
    }
}