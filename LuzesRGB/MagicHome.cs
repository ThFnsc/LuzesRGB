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

namespace LuzesRGB {
    class MagicHome : IDisposable{
        TcpClient client;
        NetworkStream nwStream;
        Task connChecker;

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;

        public IPAddress Ip;
        public int Port { get; set; }
        public bool IsConnected { get { return client != null && client.Connected; } }
        public bool TurnOnWhenConnected { get; set; }

        public MagicHome(IPAddress ip=null, int port = 5577) {
            Ip = ip;
            Port = port;
            connChecker = SetInterval(() => {
                if (!IsConnected)
                    Connect();
            }, 2000);
        }
        
        private Task SetInterval(Action action, int ms) {
            return Task.Run(() => {
                while (true) {
                    Thread.Sleep(ms);
                    action.Invoke();
                }
            });
        }

        public void Connect() {
            Task.Run(() => {
                try {
                    if (Ip == null)
                        return;
                    OnConnecting?.Invoke(this,null);
                    client?.Dispose();
                    client = new TcpClient(Ip.ToString(), Port) { NoDelay = true };
                    nwStream = client.GetStream();
                    if (TurnOnWhenConnected)
                        Turn(true);
                    OnConnect?.Invoke(this, null);
                } catch (Exception) {
                    OnConnectFail?.Invoke(this, null);
                }
            });
        }

        public void Turn(bool state) {
            if (state)
                SendCommand(new byte[] { 0x71, 0x23, 0x0f }, (b, l) => { });
            else
                SendCommand(new byte[] { 0x71, 0x24, 0x0f }, (b, l) => { });
        }

        public void SetColor(Color color) {
            SendCommand(new byte[] { 0x31, color.R, color.G, color.B, 0x00, 0x01, 0x01 });
        }

        public void SendCommand(byte[] msg, Action<byte[], int> reponse) {
            SendCommand(msg);
            byte[] receiveBuffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(receiveBuffer, 0, client.ReceiveBufferSize);
            Console.WriteLine("Received : " + Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead));
            reponse.Invoke(receiveBuffer, bytesRead);
        }

        public void SendCommand(byte[] msg) {
            long checksum = 0;
            foreach (byte b in msg)
                checksum += b;
            checksum &= 0xFF;
            byte[] msgWCS = new byte[msg.Length + 1];
            Buffer.BlockCopy(msg, 0, msgWCS, 0, msg.Length);
            Buffer.SetByte(msgWCS, msg.Length, (byte)checksum);
            try {
                nwStream.Write(msgWCS, 0, msgWCS.Length);
            } catch (Exception) {
                OnConnectionLost?.Invoke(this, null);
            }
        }

        public void Disconnect() {
            client.Close();
        }

        public void Dispose() {
            if (client.Connected)
                client.Close();
            nwStream?.Dispose();
            client?.Dispose();
        }
    }
}
