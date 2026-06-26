using System.Security.Claims;

namespace Progra3_Frontend.Services.Auth;

public class UsuarioSesion
{
    public bool Autenticado { get; set; }
    public string Nombre { get; set; } = "";
    public string Rol { get; set; } = "";
    public List<Claim> ObtenerClaims()
    {
        return new()
        {
            new Claim(ClaimTypes.Name, Nombre),
            new Claim(ClaimTypes.Role, Rol)
        };
    }
}