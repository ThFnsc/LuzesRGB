using ThFnsc.LoopbackRGB.Extensions;
using ThFnsc.LoopbackRGB.Services.AudioProviders;
using ThFnsc.LoopbackRGB.Services.Devices;
using ThFnsc.LoopbackRGB.Services.FFT;
using ThFnsc.LoopbackRGB.Services.History;

namespace ThFnsc.LoopbackRGB;

public class ColorWorker : BackgroundService
{
    private readonly IAudioProvider _audioProvider;
    private readonly IFFTCalculator _fftCalculator;
    private readonly IEnumerable<IColoreableDevice> _devices;
    private readonly ILogger<ColorWorker> _logger;
    private readonly OffloadedColorSampleSetter[] _colorSetters;
    private readonly HistoryAverageProvider _history;
    private static readonly int[] _portions = new[] { 10, 40, 40 };

    public ColorWorker(
        ILogger<ColorWorker> logger, 
        IAudioProvider audioProvider,
        IFFTCalculator fftCalculator,
        IEnumerable<IColoreableDevice> devices)
    {
        _audioProvider = audioProvider;
        _fftCalculator = fftCalculator;
        _devices = devices;
        _logger = logger;
        _colorSetters = _devices
            .Select(d => new OffloadedColorSampleSetter(d, logger))
            .ToArray();
        _history = new(10000, _portions.Length, 1);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _audioProvider.OnSamplesAvailable += SampleAvailable;
        _audioProvider.Start();
        return Task.CompletedTask;
    }

    private void SampleAvailable(object? sender, float[] samples)
    {
        _fftCalculator.Calculate(samples);
        var portions = FloatExtensions.AveragedPortions(samples, _portions);
        var normalizedSample = _history.AddSample(portions);
        if (normalizedSample != null)
            foreach (var device in _colorSetters)
                if (!device.TrySetColor(new(
                    normalizedSample[0].NormalizedFloatToFullRangeByte(),
                    normalizedSample[1].NormalizedFloatToFullRangeByte(),
                    normalizedSample[2].NormalizedFloatToFullRangeByte())))
                    _logger.LogWarning("Color missed");
    }
}
