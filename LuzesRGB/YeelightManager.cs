using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YeelightAPI;

namespace LuzesRGB
{
    public class YeelightManager : IDisposable, IColorizable
    {
        private Device _yeelight;

        private Color _lastColor;

        public bool TurnOnWhenConnected { get; set; }
        public IPAddress IPAddress { get; set; }

        public bool Connected => _yeelight?.IsConnected ?? false;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;

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
                if (TurnOnWhenConnected)
                    await Turn(true);
            }catch(Exception)
            {
                OnConnectFail?.Invoke(this, null);
            }
        }

        public Task Disconnect() =>
            Task.Run(() => {
                _yeelight?.Disconnect();
                OnConnectionLost?.Invoke(this, null);
            });

        public void Dispose() =>
            _yeelight?.Dispose();

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColor);

        public async Task SetColor(Color color)
        {
            Console.WriteLine(color);
            await Task.WhenAll(_yeelight?.SetRGBColor(color.R, color.G, color.B, 300), _yeelight.SetBrightness(Math.Max(color.R, Math.Max(color.G, color.B))));
            _lastColor = color;
        }

        public async Task Turn(bool state)
        {
            if (state)
                await _yeelight.TurnOn();
            else
                await _yeelight.TurnOff();
        }
    }
}
