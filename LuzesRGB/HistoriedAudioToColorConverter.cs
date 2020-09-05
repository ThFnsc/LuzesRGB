using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB
{
    class HistoriedAudioToColorConverter : IAudioToColorConverter
    {
        public event EventHandler<Color> OnColorAvailable;

        float[,] history = new float[3, 7500];
        short histPos = 0;
        float MIN_THRESHOLD = 0.04f;

        public void NewSpectrum(object sender, float[] spectrum)
        {
            float[] freqs = LowsMidsHighs(spectrum);
            for (int i = 0; i < freqs.Length; i++)
                history[i, histPos++] = freqs[i];
            if (histPos >= history.GetLength(1))
                histPos = 0;
            int[] amps = new int[freqs.Length];
            for (int i = 0; i < freqs.Length; i++)
                amps[i] = MapN(freqs[i], 0, MaxOf(history, i), 0, 255);
            amps[1] = Math.Max(amps[1] - amps[0], 0);
            amps[2] = Math.Max(amps[2] - amps[0], 0);
            OnColorAvailable?.Invoke(this, Color.FromArgb(amps[0], amps[1], amps[2]));
        }

        private float MaxOf(float[,] matrix, int index)
        {
            float max = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
                if (matrix[index, i] > max) max = matrix[index, i];
            return max;
        }

        private int MapN(float x, float minX, float maxX, float minY, float maxY)
        {
            try
            {
                return Convert.ToInt32(Math.Max(MIN_THRESHOLD, (x - minX) / (maxX - minX) * (maxY - minY) + minY));
            }
            catch (Exception) { return 0; }
        }

        private float[] LowsMidsHighs(float[] samples)
        {
            float[] freqs = new float[3];
            for (int i = 1; i < samples.Length / 2; i++)
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
