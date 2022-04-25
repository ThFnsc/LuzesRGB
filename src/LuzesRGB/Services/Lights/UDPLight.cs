using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LuzesRGB.Services.Lights
{
    public class UDPLight : ISmartLight, IDisposable
    {
        public UDPLight()
        {
            SW = Stopwatch.StartNew();
            Counter = 0;
        }

        public bool TurnOnWhenConnected { get; set; } = true;

        public IPEndPoint Endpoint { get; set; }

        public IPAddress IPAddress
        {
            get => Endpoint.Address;
            set => Endpoint = new IPEndPoint(value, 35225);
        }

        public Color Color { get; set; }

        public Socket Socket { get; set; }

        public bool Connected => Socket?.Connected ?? false;

        public Stopwatch SW { get; }
        public int Counter { get; private set; }

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;
        public event EventHandler<Color> OnColorChanged;

        public async Task<bool> Connect()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            await Socket.ConnectAsync(Endpoint);
            return Socket.Connected;
        }

        public Task Disconnect()
        {
            Dispose();
            Socket = null;
            return Task.CompletedTask;
        }

        public void Dispose() =>
            Socket.Dispose();

        public Task<Color> GetColor() => Task.FromResult(Color);

        public async Task SetColor(Color color)
        {
            if (Socket != null)
            {
                var value = new byte[] { 0xC0, color.R, color.G, color.B };
                await Socket.SendAsync(
                    buffer: new ArraySegment<byte>(value, 0, value.Length),
                    socketFlags: SocketFlags.None);
                //Console.WriteLine($"[{SW.ElapsedMilliseconds}] {++Counter}x");
            }
        }

        public Task Turn(bool state) => Task.CompletedTask;
    }
}
