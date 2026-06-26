namespace Progra3_Frontend.Model
{
    public class Impuesto
    {
        public int id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public double porcentaje { get; set; }
        public TipoImpuesto tipo { get; set; }
        public bool activo { get; set; }
    }
}