using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Progra3_Frontend.Services.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal usuarioActual = new(new ClaimsIdentity());

    private string nombreActual = "";
    private int idActual = 0;

    public static event Action<string>? OnGlobalLogout;

    public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;

        OnGlobalLogout += HandleGlobalLogout;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var idResult = await _protectedLocalStorage.GetAsync<int>("usuarioId");
            var nombreResult = await _protectedLocalStorage.GetAsync<string>("usuarioNombre");
            var rolResult = await _protectedLocalStorage.GetAsync<string>("usuarioRol");

            if (nombreResult.Success && !string.IsNullOrEmpty(nombreResult.Value) &&
                rolResult.Success && !string.IsNullOrEmpty(rolResult.Value) &&
                idResult.Success)
            {
                nombreActual = nombreResult.Value;
                idActual = idResult.Value;

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, idResult.Value.ToString()),
                    new Claim(ClaimTypes.Name, nombreResult.Value),
                    new Claim(ClaimTypes.Role, rolResult.Value)
                }, "CustomAuth");

                if (rolResult.Value?.Trim().Equals("MASTER", StringComparison.OrdinalIgnoreCase) == true)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
                }

                usuarioActual = new ClaimsPrincipal(identity);
            }
            else
            {
                usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
                nombreActual = "";
                idActual = 0;
            }
        }
        catch
        {
          
        }

        return new AuthenticationState(usuarioActual);
    }

    public async Task Login(int id, string nombre, string rol)
    {
        await _protectedLocalStorage.SetAsync("usuarioId", id);
        await _protectedLocalStorage.SetAsync("usuarioNombre", nombre);
        await _protectedLocalStorage.SetAsync("usuarioRol", rol);

        nombreActual = nombre;
        idActual = id;

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Role, rol)
            },
            "CustomAuth"
        );

        if (rol?.Trim().Equals("MASTER", StringComparison.OrdinalIgnoreCase) == true)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
        }

        usuarioActual = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        string usuarioQueSale = nombreActual;

        await _protectedLocalStorage.DeleteAsync("usuarioId");
        await _protectedLocalStorage.DeleteAsync("usuarioNombre");
        await _protectedLocalStorage.DeleteAsync("usuarioRol");

        usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
        nombreActual = "";
        idActual = 0;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        if (!string.IsNullOrEmpty(usuarioQueSale))
        {
            OnGlobalLogout?.Invoke(usuarioQueSale);
        }
    }

    private void HandleGlobalLogout(string nombreUsuarioQueSalio)
    {
        if (!string.IsNullOrEmpty(nombreActual) && nombreActual == nombreUsuarioQueSalio)
        {
            usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
            nombreActual = "";
            idActual = 0;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(usuarioActual)));
        }
    }

    public void Dispose()
    {
        OnGlobalLogout -= HandleGlobalLogout;
    }
}
