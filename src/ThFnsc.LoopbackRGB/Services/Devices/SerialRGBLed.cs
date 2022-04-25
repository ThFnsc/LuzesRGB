using System.IO.Ports;
using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.Devices;

public class SerialRGBLed : IColoreableDevice, IDisposable
{
    private bool _disposed = false;
    private SerialPort _serial;
    private readonly ILogger<SerialRGBLed> _logger;
    private readonly byte[] _colorPayload = new byte[3];
    private static readonly byte[] _ack = new byte[] { 1, 2, 3, 4 };
    
    public SerialRGBLed(ILogger<SerialRGBLed> logger)
    {
        _logger = logger;
        _serial = GetSerialPort();
    }

    private SerialPort GetSerialPort()
    {
        foreach (var chosenSerialPort in SerialPort.GetPortNames())
        {
            var serialPort = new SerialPort(chosenSerialPort, 115200)
            {
                ReadTimeout = 5
            };
            try
            {
                serialPort.Open();
                if (!TestSerialPort(serialPort))
                {
                    serialPort.Close();
                    _logger.LogInformation("Serial port {SerialPort} is not a RGB LED", chosenSerialPort);
                    continue;
                }
                _logger.LogInformation("Found a RGB LED in serial port {SerialPort}", chosenSerialPort);
                return serialPort;
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogInformation("Serial port {SerialPort} is being used", chosenSerialPort);
            }                
        }
        throw new Exception("No device found");
    }

    private static bool TestSerialPort(SerialPort serialPort)
    {
        var escapedAck = EscapedBinaryProtocol.Write(_ack);
        serialPort.Write(escapedAck, 0, escapedAck.Length);
        var memory = new MemoryStream();
        var writer = new BinaryWriter(memory);
        try
        {
            while (true)
            {
                var read = serialPort.ReadByte();
                if (read >= 0)
                    writer.Write((byte)read);
            }
        }
        catch (TimeoutException)
        {
            memory.Position = 0;
            try
            {
                var message = EscapedBinaryProtocol.Read(memory);
                return message.SequenceEqual(_ack);
            }
            catch
            {
                return false;
            }
        }
    }

    public void SetColor(RGBColor color)
    {
        if (!_serial.IsOpen)
            throw new InvalidOperationException("Color cannot be set. Serial device unavailable");
        _colorPayload[0] = color.Red;
        _colorPayload[1] = color.Green;
        _colorPayload[2] = color.Blue;
        var converted = EscapedBinaryProtocol.Write(_colorPayload);
        _serial.Write(converted, 0, converted.Length);
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