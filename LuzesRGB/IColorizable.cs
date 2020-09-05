using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB
{
    public interface IColorizable
    {
        event EventHandler OnConnecting;
        event EventHandler OnConnect;
        event EventHandler OnConnectionLost;
        event EventHandler OnConnectFail;
        bool TurnOnWhenConnected { get; set; }
        IPAddress IPAddress { get; set; }
        Task Connect();
        bool Connected { get; }
        Task Turn(bool state);
        Task Disconnect();
        Task SetColor(Color color);
        Task<Color> GetColor();
    }
}
