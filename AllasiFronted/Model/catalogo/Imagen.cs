namespace Progra3_Frontend.Model
{
    public class Imagen
    {
        public int id { get; set; }
        public string url { get; set; } = string.Empty;
        public bool principal { get; set; }

        public Imagen() { }
        public Imagen(string url)
        {
            this.url = url;
        }
    }
}
