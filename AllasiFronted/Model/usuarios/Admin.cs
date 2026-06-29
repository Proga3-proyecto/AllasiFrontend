namespace Progra3_Frontend.Model
{
    public class Admin : Usuario
    {
        public DateTime? fechaInicioAdmin { get; set; }

        public bool is_master { get; set; }
        public Admin()
        {
            TipoUsuario = "Admin";
        }
    }
}