using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Progra3_Frontend.Services.Auth;

public class CustomAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    private ClaimsPrincipal usuarioActual =
        new(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        try
        {
            var nombreResult = await _protectedLocalStorage.GetAsync<string>("usuarioNombre");
            var rolResult = await _protectedLocalStorage.GetAsync<string>("usuarioRol");

            if (nombreResult.Success && !string.IsNullOrEmpty(nombreResult.Value) &&
                rolResult.Success && !string.IsNullOrEmpty(rolResult.Value))
            {
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
            }
        }
        catch
        {
            // Catch for prerendering where JS interop is not available
        }

        return new AuthenticationState(usuarioActual);
    }

    public async Task Login(string nombre, string rol)
    {
        await _protectedLocalStorage.SetAsync("usuarioNombre", nombre);
        await _protectedLocalStorage.SetAsync("usuarioRol", rol);

        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Role, rol)
            },
            "CustomAuth"
        );

        usuarioActual =
            new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }

    public async Task Logout()
    {
        await _protectedLocalStorage.DeleteAsync("usuarioNombre");
        await _protectedLocalStorage.DeleteAsync("usuarioRol");

        usuarioActual =
            new ClaimsPrincipal(
                new ClaimsIdentity());

        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }
}