using ThFnsc.LoopbackRGB.Extensions;
using ThFnsc.LoopbackRGB.Models;
using ThFnsc.LoopbackRGB.Services.AudioProviders;
using ThFnsc.LoopbackRGB.Services.ColorProcessors;
using ThFnsc.LoopbackRGB.Services.Devices;
using ThFnsc.LoopbackRGB.Services.FFT;
using ThFnsc.LoopbackRGB.Services.History;

namespace ThFnsc.LoopbackRGB;

public class ColorWorker : BackgroundService
{
    private readonly IAudioProvider _audioProvider;
    private readonly IFFTCalculator _fftCalculator;
    private readonly IEnumerable<IColoreableDevice> _devices;
    private readonly IReadOnlyList<IColorProcessor> _colorProcessors;
    private readonly ILogger<ColorWorker> _logger;
    private readonly OffloadedColorSampleSetter[] _colorSetters;
    private readonly HistoryAverageProvider _history;
    private static readonly int[] _portions = new[] { 1, 2, 18 };

    public ColorWorker(
        ILogger<ColorWorker> logger,
        IAudioProvider audioProvider,
        IFFTCalculator fftCalculator,
        IEnumerable<IColoreableDevice> devices,
        IEnumerable<IColorProcessor> colorProcessors)
    {
        _audioProvider = audioProvider;
        _fftCalculator = fftCalculator;
        _devices = devices;
        _colorProcessors = colorProcessors.OrderBy(p => p.Order).ToArray();
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

    private void SampleAvailable(object? sender, float[][] samples)
    {
        foreach (var channel in samples)
            _fftCalculator.Calculate(channel);

        var portions = samples.Select(c => FloatExtensions.AveragedPortions(c, _portions));
        var colors = portions.Select(c => _history.AddSample(c)?.Select(FloatExtensions.NormalizedFloatToFullRangeByte).ToArray())
            .WhereNotNull()
            .Select(RGBColor.FromByteArray)
            .ToArray();

        for (var i = 0; i < colors.Length; i++)
            foreach (var colorProcessor in _colorProcessors)
                colors[i] = colorProcessor.Process(colors[i]);

        if (colors.Length > 0)
            foreach (var device in _colorSetters)
                if (!device.TrySetColors(colors))
                    _logger.LogWarning("Color missed");
    }
}