using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB.Services.Lights
{
    public class SerialLight : ISmartLight
    {
        private readonly SerialPort _serial;
        public SerialLight()
        {
            var serialPorts = SerialPort.GetPortNames();

            var chosenSerialPort = serialPorts.LastOrDefault();

            if (!string.IsNullOrWhiteSpace(chosenSerialPort))
            {
                _serial = new SerialPort(chosenSerialPort, 115200);
            }
        }

        private Color _lastColor;

        public bool TurnOnWhenConnected { get; set; }
        public IPAddress IPAddress { get; set; }
        public bool Connected { get; }

        public event EventHandler OnConnecting;
        public event EventHandler OnConnect;
        public event EventHandler OnConnectionLost;
        public event EventHandler OnConnectFail;
        public event EventHandler<Color> OnColorChanged;

        public Task<bool> Connect()
        {
            if (!_serial.IsOpen)
                _serial.Open();
            return Task.FromResult(true);
        }

        public Task Disconnect()
        {
            if (_serial.IsOpen)
                _serial.Close();
            return Task.CompletedTask;
        }

        public Task<Color> GetColor() =>
            Task.FromResult(_lastColor);

        private byte ColorToShiftedByte(byte color) =>
            (byte)((color >> 1) | 0b10000000);

        public Task SetColor(Color color)
        {
            _lastColor = color;
            var message = new byte[] {
                ColorToShiftedByte(color.R),
                ColorToShiftedByte(color.G),
                ColorToShiftedByte(color.B),
                0
            };
            //Console.WriteLine(string.Join(", ", message.Select(b => Convert.ToString(b, 2).PadLeft(8))));
            if (_serial.IsOpen)
                _serial?.Write(message, 0, message.Length);
            return Task.CompletedTask;
        }

        public Task Turn(bool state) =>
            Task.CompletedTask;
    }
}
