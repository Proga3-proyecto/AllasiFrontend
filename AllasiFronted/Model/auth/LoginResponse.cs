namespace Progra3_Frontend.Models
{
    public class LoginResponse
    {
        public int IdUsuario { get; set; }

        public string Nombre { get; set; } = "";

        public string Rol { get; set; } = "";

        public bool Exito { get; set; }

        public string Mensaje { get; set; } = "";
    }
}
