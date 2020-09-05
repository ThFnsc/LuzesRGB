using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;

namespace LuzesRGB
{
    class MagicHomeLEDStrip : IDisposable, IColorizable
    {
        TcpClient client;
        NetworkStream nwStream;
        private readonly Task connChecker;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;

        public IPAddress IPAddress { get; set; }
        public int Port { get; set; }
        public bool Connected { get { return client != null && client.Connected; } }
        public bool TurnOnWhenConnected { get; set; }
        private Color _lastColorSent;

        public async Task SetColor(Color color)
        {
            if (!_lastColorSent.Equals(color))
                await SendCommand(new byte[] { 0x31, color.R, color.G, color.B, 0x00, 0x01, 0x01 });
            _lastColorSent = color;
        }

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColorSent);

        public MagicHomeLEDStrip(IPAddress ip = null, int port = 5577)
        {
            IPAddress = ip;
            Port = port;
            connChecker = SetInterval(async () =>
            {
                if (!Connected)
                    await Connect();
            }, 2000);
        }

        private Task SetInterval(Action action, int ms)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(ms);
                    action.Invoke();
                }
            });
        }

        public async Task Connect()
        {
            try
            {
                if (IPAddress == null)
                    return;
                OnConnecting?.Invoke(this, null);
                client?.Dispose();
                client = new TcpClient(IPAddress.ToString(), Port) { NoDelay = true };
                nwStream = client.GetStream();
                if (TurnOnWhenConnected)
                    await Turn(true);
                OnConnect?.Invoke(this, null);
            }
            catch (Exception)
            {
                OnConnectFail?.Invoke(this, null);
            }
        }

        public async Task Turn(bool state)
        {
            if (state)
                await SendCommand(new byte[] { 0x71, 0x23, 0x0f }, (b, l) => { });
            else
                await SendCommand(new byte[] { 0x71, 0x24, 0x0f }, (b, l) => { });
        }

        public async Task SendCommand(byte[] msg, Action<byte[], int> reponse)
        {
            await SendCommand(msg);
            byte[] receiveBuffer = new byte[client.ReceiveBufferSize];
            int bytesRead = await nwStream.ReadAsync(receiveBuffer, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead));
            reponse.Invoke(receiveBuffer, bytesRead);
        }

        public async Task SendCommand(byte[] msg)
        {
            long checksum = 0;
            foreach (byte b in msg)
                checksum += b;
            checksum &= 0xFF;
            byte[] msgWCS = new byte[msg.Length + 1];
            Buffer.BlockCopy(msg, 0, msgWCS, 0, msg.Length);
            Buffer.SetByte(msgWCS, msg.Length, (byte)checksum);
            try
            {
                await nwStream?.WriteAsync(msgWCS, 0, msgWCS.Length);
            }
            catch (Exception)
            {
                OnConnectionLost?.Invoke(this, null);
            }
        }

        public Task Disconnect()=>
            Task.Run(()=> client.Close());

        public void Dispose()
        {
            connChecker?.Dispose();
            nwStream?.Flush();
            if (client.Connected)
                client.Close();
            nwStream?.Dispose();
            client?.Dispose();
        }
    }
}