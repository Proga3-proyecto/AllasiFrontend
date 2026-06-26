namespace Progra3_Frontend.Model
{
    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;


        public Categoria() { }

        public Categoria(string categoria)
        {
            this.nombre = categoria;
        }
    }
}
