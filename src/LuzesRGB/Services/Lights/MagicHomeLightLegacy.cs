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

namespace LuzesRGB.Services.Lights
{
    public class MagicHomeLightLegacy : IDisposable, ISmartLight
    {
        TcpClient client;
        NetworkStream nwStream;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;
        public event EventHandler<Color> OnColorChanged;

        public IPAddress IPAddress { get; set; }
        public int Port { get; set; } = 5577;
        public bool Connected { get { return client != null && client.Connected; } }
        public bool TurnOnWhenConnected { get; set; }
        private Color _lastColorSent;

        public async Task SetColor(Color color)
        {
            if (!_lastColorSent.Equals(color))
                await SendCommand(new byte[] { 0x31, color.R, color.G, color.B, 0x00, 0x01, 0x01 });
            _lastColorSent = color;
            OnColorChanged?.Invoke(this, color);
        }

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColorSent);

        public async Task<bool> Connect()
        {
            try
            {
                if (IPAddress == null)
                    return false;
                OnConnecting?.Invoke(this, null);
                client?.Dispose();
                client = new TcpClient(IPAddress.ToString(), Port) { NoDelay = true };
                nwStream = client.GetStream();
                if (TurnOnWhenConnected)
                    await Turn(true);
                OnConnect?.Invoke(this, null);
                return Connected;
            }
            catch (Exception)
            {
                OnConnectFail?.Invoke(this, null);
                return false;
            }
        }

        public Task Turn(bool state)=>
            SendCommand(new byte[] { 0x71, (byte)(state ? 0x23 : 0x24), 0x0f }, (b, l) => { });

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
                if(nwStream!=null)
                    await nwStream.WriteAsync(msgWCS, 0, msgWCS.Length);
            }
            catch (Exception)
            {
                OnConnectionLost?.Invoke(this, null);
            }
        }

        public Task Disconnect() =>
            Task.Run(() => client.Close());

        public void Dispose()
        {
            nwStream?.Flush();
            if (client.Connected)
                client.Close();
            nwStream?.Dispose();
            client?.Dispose();
        }
    }
}