namespace Progra3_Frontend.Model
{
    public class Receta
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string instrucciones { get; set; }
        public double precio { get; set; }
        public double precioFinal { get; set; }
        public double descuento { get; set; }
        public List<ElementoReceta> elementos { get; set; } = new List<ElementoReceta>();
        public List<Imagen> imagenes { get; set; } = new List<Imagen>();
        public List<Categoria> categorias { get; set; } = new List<Categoria>();

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
