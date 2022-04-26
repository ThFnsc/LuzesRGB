using NAudio.Wave;
using System.Diagnostics;

namespace ThFnsc.LoopbackRGB.Services.AudioProviders;

public class NAudioLoopbackAudioProvider : IAudioProvider, IDisposable
{
    private static readonly TimeSpan _maxSilenceDuration = TimeSpan.FromSeconds(5);
    private const int _sampleSize = 1 << 11;
    private const int _channels = 2;

    private readonly float[][] _samples = Enumerable.Range(0, _channels).Select(_ => new float[_sampleSize]).ToArray();
    private readonly ILogger<NAudioLoopbackAudioProvider> _logger;
    private byte[]? _buffer;
    private readonly Stopwatch _silenceStopwatch = Stopwatch.StartNew();
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    private WasapiLoopbackCapture? _audio;
    private BufferedWaveProvider? _bwp;

    public event EventHandler<float[][]>? OnSamplesAvailable;

    public NAudioLoopbackAudioProvider(ILogger<NAudioLoopbackAudioProvider> logger)
    {
        _logger = logger;
        _ = CheckSilence();
    }

    private async Task CheckSilence()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            if (_silenceStopwatch.Elapsed > _maxSilenceDuration)
            {
                Restart();
                _silenceStopwatch.Restart();
            }
            await Task.Delay(TimeSpan.FromSeconds(1), _cancellationTokenSource.Token);
        }
    }

    public void Start()
    {
        _audio = new WasapiLoopbackCapture();
        _bwp = new BufferedWaveProvider(_audio.WaveFormat) { DiscardOnBufferOverflow = true };
        _buffer = new byte[_sampleSize * _audio.WaveFormat.BlockAlign];
        _audio.DataAvailable += (_, e) =>
        {
            _bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
            while (_bwp.BufferedBytes >= _buffer.Length)
            {
                _bwp.Read(_buffer, 0, _buffer.Length);
                var somethingPlaying = _buffer.Any(b => b != 0);
                if (somethingPlaying)
                    _silenceStopwatch.Restart();
                for (int i = 0, offset = 0; i < _sampleSize; i++, offset += _audio.WaveFormat.BlockAlign)
                    for (var j = 0; j < _channels; j++)
                        _samples[j][i] = BitConverter.ToSingle(_buffer, offset + sizeof(float) * j);
                OnSamplesAvailable?.Invoke(this, _samples);
            }
        };
        _audio.RecordingStopped += (_, e) =>
        {
            if (e.Exception != null)
                _logger.LogError(e.Exception, "Recording stopped");
            Restart();
        };
        _audio.StartRecording();
    }

    private void Restart()
    {
        ArgumentNullException.ThrowIfNull(_audio);
        if (_audio.CaptureState is NAudio.CoreAudioApi.CaptureState.Capturing)
            _audio.StopRecording();
        else
        {
            _audio.Dispose();
            if (!_cancellationTokenSource.IsCancellationRequested)
                Start();
        }
    }

    public void Dispose()
    {
        if (!_cancellationTokenSource.IsCancellationRequested)
            _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        if (_audio is not null)
            Restart();
    }
}
