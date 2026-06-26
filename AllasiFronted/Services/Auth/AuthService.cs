using System.Net.Http.Json;
using Progra3_Frontend.Models;

namespace Progra3_Frontend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient http;

        public AuthService(HttpClient http)
        {
            this.http = http;
        }

        public async Task<LoginResponse?> Login(
            LoginRequest request)
        {
            var response =
                await http.PostAsJsonAsync(
                    "auth/login",
                    request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content
                .ReadFromJsonAsync<LoginResponse>();
        }
    }
}
