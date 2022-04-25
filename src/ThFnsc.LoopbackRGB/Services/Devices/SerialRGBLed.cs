using System.IO.Ports;
using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.Devices;

public class SerialRGBLed : IColoreableDevice, IDisposable
{
    private bool _disposed = false;
    private SerialPort _serial;
    private readonly byte[] _colorPayload = new byte[4];

    public SerialRGBLed()
    {
        _serial = GetSerialPort();
    }

    private static SerialPort GetSerialPort()
    {
        var serialPorts = SerialPort.GetPortNames();

        var chosenSerialPort = serialPorts.LastOrDefault();

        if (!string.IsNullOrWhiteSpace(chosenSerialPort))
        {
            var serialPort = new SerialPort(chosenSerialPort, 115200);
            serialPort.Open();
            return serialPort;
        }
        else 
            throw new InvalidOperationException("Serial port not found");
    }

    private byte ColorToShiftedByte(byte color) =>
        (byte)(color >> 1 | 0b10000000);

    public void SetColor(RGBColor color)
    {
        if (!_serial.IsOpen)
            throw new InvalidOperationException("Color cannot be set. Serial device unavailable");
        _colorPayload[0] = ColorToShiftedByte(color.Red);
        _colorPayload[1] = ColorToShiftedByte(color.Green);
        _colorPayload[2] = ColorToShiftedByte(color.Blue);
        _serial.Write(_colorPayload, 0, _colorPayload.Length);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        if (_serial is not null)
        {
            if (_serial.IsOpen)
            {
                SetColor(new(0, 0, 0));
                _serial.Close();
            }
            _serial.Dispose();
        }
    }
}