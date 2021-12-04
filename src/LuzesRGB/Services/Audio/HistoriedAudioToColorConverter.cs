using System;
using System.Drawing;

namespace LuzesRGB
{
    internal class HistoriedAudioToColorConverter : IAudioToColorConverter
    {
        public event EventHandler<Color> OnColorAvailable;

        private readonly float[,] _history = new float[3, 7500];
        private readonly float _mIN_THRESHOLD = 0.04f;
        private short _histPos = 0;

        public void NewSpectrum(object sender, float[] spectrum)
        {
            var freqs = LowsMidsHighs(spectrum);
            for (var i = 0; i < freqs.Length; i++)
                _history[i, _histPos++] = freqs[i];
            if (_histPos >= _history.GetLength(1))
                _histPos = 0;
            var amps = new int[freqs.Length];
            for (var i = 0; i < freqs.Length; i++)
                amps[i] = MapN(freqs[i], 0, MaxOf(_history, i), 0, 255);
            amps[1] = Math.Max(amps[1] - amps[0] / 3, 0);
            amps[2] = Math.Max(amps[2] - amps[0] / 3, 0);
            OnColorAvailable?.Invoke(this, Color.FromArgb(amps[0], amps[1], amps[2]));
        }

        private float MaxOf(float[,] matrix, int index)
        {
            float max = 0;
            for (var i = 0; i < matrix.GetLength(1); i++)
                if (matrix[index, i] > max) max = matrix[index, i];
            return max;
        }

        private int MapN(float x, float minX, float maxX, float minY, float maxY)
        {
            try
            {
                return Convert.ToInt32(Math.Max(_mIN_THRESHOLD, (x - minX) / (maxX - minX) * (maxY - minY) + minY));
            }
            catch (Exception) { return 0; }
        }

        private float[] LowsMidsHighs(float[] samples)
        {
            var freqs = new float[3];
            for (var i = 1; i < samples.Length / 2; i++)
            {
                if (i < 10)
                {
                    if (samples[i] > freqs[0]) freqs[0] = samples[i];
                }
                else if (i < 200)
                {
                    if (samples[i] > freqs[1]) freqs[1] = samples[i];
                }
                else
                {
                    if (samples[i] > freqs[2]) freqs[2] = samples[i];
                }
            }
            return freqs;
        }
    }
}
