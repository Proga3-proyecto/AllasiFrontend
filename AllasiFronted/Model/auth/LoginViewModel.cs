using System.ComponentModel.DataAnnotations;

namespace Progra3_Frontend.Models
{
    public class LoginViewModel
    {
        public string Usuario { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
