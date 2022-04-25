namespace ThFnsc.LoopbackRGB.Services.History;
public class HistoryAverageProvider
{
    private readonly int _maxSamples;
    private readonly int _sampleSize;
    private readonly float[,] _samples;
    private int _sampleCount = 0, _sampleIndex = 0;

    private readonly float[] _mins;
    private readonly float[] _maxs;
    private readonly int _minHistoryToYield;

    public HistoryAverageProvider(int samples, int sampleSize, int minHistoryToYield)
    {
        _maxSamples = samples;
        _sampleSize = sampleSize;
        _samples = new float[samples, sampleSize];
        _mins = new float[sampleSize];
        _maxs = new float[sampleSize];
        _minHistoryToYield = minHistoryToYield;
    }

    public float[]? AddSample(float[] inputSamples)
    {
        lock (this)
        {
            for (int i = 0; i < inputSamples.Length; i++)
                _samples[_sampleIndex, i] = inputSamples[i];
            if (_sampleCount < _maxSamples)
                _sampleCount++;
            _sampleIndex = (_sampleIndex + 1) % _maxSamples;

            if (_sampleCount < _minHistoryToYield)
                return null;

            for (var i = 0; i < _sampleSize; i++)
            {
                _mins[i] = float.MaxValue;
                _maxs[i] = float.MinValue;
            }

            for (var i = 0; i < _sampleCount; i++)
                for (var j = 0; j < _sampleSize; j++)
                {
                    var sample = _samples[i, j];
                    if (sample < _mins[j])
                        _mins[j] = sample;
                    if (sample > _maxs[j])
                        _maxs[j] = sample;
                }

            var results = new float[_sampleSize];
            for (var i = 0; i < _sampleSize; i++)
            {
                var min = _mins[i];
                var max = _maxs[i];
                var range = max - min;
                results[i] = range == 0
                    ? 1
                    : (inputSamples[i] - min) / range;
            }

            return results;
        }
    }
}
