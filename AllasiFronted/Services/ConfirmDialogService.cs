using System;
using System.Threading.Tasks;

namespace Progra3_Frontend.Services
{
    public enum DialogType
    {
        Danger,
        Info,
        Success,
        Warning
    }

    public class ConfirmDialogService
    {
        public event Func<string, string, DialogType, Task<bool>>? OnShow;

        public async Task<bool> ConfirmAsync(string title, string message, DialogType type = DialogType.Danger)
        {
            if (OnShow != null)
            {
                return await OnShow.Invoke(title, message, type);
            }
            return false;
        }
    }
}
