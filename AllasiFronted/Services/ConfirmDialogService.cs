using System;
using System.Threading.Tasks;

namespace Progra3_Frontend.Services
{
    public class ConfirmDialogService
    {
        public event Func<string, string, Task<bool>>? OnShow;

        public async Task<bool> ConfirmAsync(string title, string message)
        {
            if (OnShow != null)
            {
                return await OnShow.Invoke(title, message);
            }
            return false;
        }
    }
}
