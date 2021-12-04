using System;
using System.Net;
using System.Threading.Tasks;

namespace LuzesRGB.Services.Lights
{
    public interface ISmartLight : IColorizable
    {
        event EventHandler OnConnecting;
        event EventHandler OnConnect;
        event EventHandler OnConnectionLost;
        event EventHandler OnConnectFail;
        bool TurnOnWhenConnected { get; set; }
        IPAddress IPAddress { get; set; }
        Task<bool> Connect();
        bool Connected { get; }
        Task Turn(bool state);
        Task Disconnect();
    }
}
