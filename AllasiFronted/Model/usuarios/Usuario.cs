using System.Text.Json.Serialization;

namespace Progra3_Frontend.Model
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string dni { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellidoCompleto { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string contrasenaHash { get; set; } = string.Empty;
        public EstadoUsuario estado { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

        [JsonIgnore]
        public string TipoUsuario { get; set; } = "Usuario";
    }
}