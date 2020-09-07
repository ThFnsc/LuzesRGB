using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB
{
    class LoopbackAudio : IAudioProvider
    {
        private WasapiLoopbackCapture _audio;
        private BufferedWaveProvider _bwp;
        private DateTime? _lastSilence;
        private static readonly int BUFFER_SIZE = (int)Math.Pow(2, 15);
        private static readonly TimeSpan MAX_SILENCE = TimeSpan.FromSeconds(10);

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        public event EventHandler<float[]> OnAudioData;

        private void Calculate()
        {
            if (_bwp.BufferedBytes < BUFFER_SIZE)
                return;
            byte[] buffer = new byte[BUFFER_SIZE];
            _bwp.Read(buffer, 0, BUFFER_SIZE);
            float[] leftSamples = new float[BUFFER_SIZE / 8];
            float[] rightSamples = new float[BUFFER_SIZE / 8];
            float[] joinedSamples = new float[BUFFER_SIZE / 8];

            for (int i = 0; i < BUFFER_SIZE; i += 8)
            {
                leftSamples[i / 8] = BitConverter.ToSingle(buffer, i);
                rightSamples[i / 8] = BitConverter.ToSingle(buffer, i + 4);
                joinedSamples[i / 8] = (leftSamples[i / 8] + rightSamples[i / 8]) / 2;
            }

            OnAudioData?.Invoke(this, FFT(joinedSamples));
        }

        private float[] FFT(float[] data)
        {
            float[] fft = new float[data.Length];
            Complex[] fftComplex = new Complex[data.Length];
            for (int i = 0; i < data.Length; i++)
                fftComplex[i] = new Complex(data[i], 0.0);
            Accord.Math.Transforms.FourierTransform2.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Backward);
            for (int i = 0; i < data.Length; i++)
                fft[i] = (float)fftComplex[i].Magnitude;
            return fft;
        }

        public void Dispose() =>
            Task.WhenAny(Task.Run(()=> _audio?.Dispose()), Task.Delay(10000)).Wait();

        public Task Start()
        {
            _audio = new WasapiLoopbackCapture();
            _bwp = new BufferedWaveProvider(_audio.WaveFormat) { DiscardOnBufferOverflow = true, BufferLength = BUFFER_SIZE * 2 };
            _audio.DataAvailable += (sndr, args) =>
            {
                _bwp.AddSamples(args.Buffer, 0, args.BytesRecorded);
                Calculate();
                if (args.BytesRecorded == 0)
                {
                    if (_lastSilence == null)
                        _lastSilence = DateTime.Now;
                    else if (DateTime.Now - _lastSilence > MAX_SILENCE)
                    {
                        _lastSilence = null;
                        Stop();
                        Start();
                    }
                }
                else
                    _lastSilence = null;
            };
            _audio.StartRecording();
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
