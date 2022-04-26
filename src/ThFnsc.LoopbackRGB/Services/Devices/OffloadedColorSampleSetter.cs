using System.Collections.Concurrent;
using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.Devices;

public class OffloadedColorSampleSetter
{
    private readonly IColoreableDevice _device;
    private readonly BlockingCollection<RGBColor[]> _colorQueue;
    private readonly ILogger _logger;
    private readonly Thread _thread;

    public OffloadedColorSampleSetter(IColoreableDevice device, ILogger logger)
    {
        _device = device;
        _colorQueue = new(2);
        _logger = logger;
        _thread = new Thread(new ThreadStart(Worker))
        {
            Name = $"Background color setter for device: {device}"
        };
        _thread.Start();
    }

    private void Worker()
    {
        foreach (var colors in _colorQueue.GetConsumingEnumerable())
            try
            {
                _device.SetColors(colors);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error setting color on device: {_device}");
            }
    }

    public bool TrySetColors(RGBColor[] colors) =>
        _colorQueue.TryAdd(colors);
}
