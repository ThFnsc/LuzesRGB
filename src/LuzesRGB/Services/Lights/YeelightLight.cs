using System;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using YeelightAPI;

namespace LuzesRGB.Services.Lights
{
    public class YeelightLight : IDisposable, ISmartLight
    {
        private Device _yeelight;

        private Color _lastColor;

        public bool TurnOnWhenConnected { get; set; } = true;
        public IPAddress IPAddress { get; set; }

        public bool Connected => _yeelight?.IsConnected ?? false;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;
        public event EventHandler<Color> OnColorChanged;

        public async Task Connect()
        {
            try
            {
                _yeelight?.Dispose();
                OnConnecting?.Invoke(this, null);
                var ip = IPAddress.ToString();
                _yeelight = new Device(ip);
                await _yeelight.Connect();
                await _yeelight.StartMusicMode();
                OnConnect?.Invoke(this, null);
                if (TurnOnWhenConnected)
                    await Turn(true);
            }
            catch (Exception)
            {
                OnConnectFail?.Invoke(this, null);
            }
        }

        public Task Disconnect() =>
            Task.Run(() =>
            {
                _yeelight?.Disconnect();
                OnConnectionLost?.Invoke(this, null);
            });

        public void Dispose() =>
            _yeelight?.Dispose();

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColor);

        public async Task SetColor(Color color)
        {
            if (_yeelight != null)
                await Task.WhenAll(_yeelight.SetRGBColor(color.R, color.G, color.B, 300), _yeelight.SetBrightness(Math.Max(color.R, Math.Max(color.G, color.B))));
            _lastColor = color;
            OnColorChanged?.Invoke(this, color);
        }

        public async Task Turn(bool state)
        {
            if (state)
                await _yeelight.TurnOn();
            else
                await _yeelight.TurnOff();
        }

        Task<bool> ISmartLight.Connect() => throw new NotImplementedException();
    }
}
