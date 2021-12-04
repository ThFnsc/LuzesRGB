using MagicHome;
using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;

namespace LuzesRGB.Services.Lights
{
    public class MagicHomeLight : ISmartLight, IDisposable
    {
        private Light _light;
        private Color _lastColor;

        public bool TurnOnWhenConnected { get; set; } = true;
        public IPAddress IPAddress { get; set; }

        public bool Connected => _light?.Connected ?? false;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;
        public event EventHandler<Color> OnColorChanged;

        public async Task<bool> Connect()
        {
            try
            {
                await Disconnect();
                OnConnecting?.Invoke(this, null);
                _light = new Light(IPAddress)
                {
                    InitialPowerState = TurnOnWhenConnected,
                    UseChecksum = true,
                    AutoRefreshEnabled = true,
                    Timeout = TimeSpan.FromSeconds(5),
                    AutoRefreshInterval = TimeSpan.FromSeconds(2)
                };
                await _light.ConnectAsync();
                if (TurnOnWhenConnected)
                    await Turn(true);
                OnConnect?.Invoke(this, null);
                return Connected;
            }
            catch (Exception)
            {
                OnConnectFail?.Invoke(this, null);
                _light?.Dispose();
                return false;
            }
        }

        public Task Disconnect() =>
            Task.Run(() =>
            {
                _light?.Dispose();
                OnConnectionLost?.Invoke(this, null);
                return Task.CompletedTask;
            });

        public void Dispose() =>
            _light?.Dispose();

        public async Task SetColor(System.Drawing.Color color)
        {
            if (_light != null)
                await _light.SetColorAsync(color);
            OnColorChanged?.Invoke(this, color);
            _lastColor = color;
        }

        public async Task Turn(bool state)
        {
            for (byte i = 0; i < 3; i++, await Task.Delay(100))
                await _light.SetPowerAsync(state);
        }

        Task<System.Drawing.Color> IColorizable.GetColor() =>
            Task.FromResult(_lastColor);
    }
}
