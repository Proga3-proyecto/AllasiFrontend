using Progra3_Frontend.Models;

namespace Progra3_Frontend.Services.Auth
{
    public interface IAuthService
    {
        public Task<LoginResponse?> Login(LoginRequest request);
    }
}
