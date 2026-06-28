using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Progra3_Frontend.Services.Auth;

// 1. Agregamos IDisposable para evitar fugas de memoria con el evento estático
public class CustomAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal usuarioActual = new(new ClaimsIdentity());

    private string nombreActual = "";

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
            var nombreResult = await _protectedLocalStorage.GetAsync<string>("usuarioNombre");
            var rolResult = await _protectedLocalStorage.GetAsync<string>("usuarioRol");

            if (nombreResult.Success && !string.IsNullOrEmpty(nombreResult.Value) &&
                rolResult.Success && !string.IsNullOrEmpty(rolResult.Value))
            {
                nombreActual = nombreResult.Value; // Actualizamos la variable interna

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nombreResult.Value),
                    new Claim(ClaimTypes.Role, rolResult.Value)
                }, "CustomAuth");

                usuarioActual = new ClaimsPrincipal(identity);
            }
            else
            {
                usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
                nombreActual = "";
            }
        }
        catch
        {
          
        }

        return new AuthenticationState(usuarioActual);
    }

    public async Task Login(string nombre, string rol)
    {
        await _protectedLocalStorage.SetAsync("usuarioNombre", nombre);
        await _protectedLocalStorage.SetAsync("usuarioRol", rol);

        nombreActual = nombre;

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Role, rol)
            },
            "CustomAuth"
        );

        usuarioActual = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        string usuarioQueSale = nombreActual;

        await _protectedLocalStorage.DeleteAsync("usuarioNombre");
        await _protectedLocalStorage.DeleteAsync("usuarioRol");

        usuarioActual = new ClaimsPrincipal(new ClaimsIdentity());
        nombreActual = "";

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

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(usuarioActual)));
        }
    }

    public void Dispose()
    {
        OnGlobalLogout -= HandleGlobalLogout;
    }
}