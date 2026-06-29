using System;
using System.Timers;

namespace Progra3_Frontend.Services
{
    public class ToastService
    {
        public event Action<string, string>? OnShow;
        public event Action? OnHide;
        private System.Timers.Timer? _timer;

        public void ShowToast(string message, string type = "error", int durationMs = 2000)
        {
            OnShow?.Invoke(message, type);
            
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }

            _timer = new System.Timers.Timer(durationMs);
            _timer.Elapsed += (s, e) =>
            {
                OnHide?.Invoke();
                _timer.Stop();
                _timer.Dispose();
            };
            _timer.Start();
        }
    }
}
