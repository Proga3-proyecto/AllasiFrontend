using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Progra3_Frontend.Services.Auth;

public class CustomAuthenticationStateProvider
    : AuthenticationStateProvider
{
    private ClaimsPrincipal usuarioActual =
        new(new ClaimsIdentity());

    public override Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {
        return Task.FromResult(
            new AuthenticationState(usuarioActual));
    }

    public void Login(string nombre, string rol)
    {
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

    public void Logout()
    {
        usuarioActual =
            new ClaimsPrincipal(
                new ClaimsIdentity());

        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());
    }
}