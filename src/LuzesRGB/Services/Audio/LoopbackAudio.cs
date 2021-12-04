using NAudio.Wave;
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LuzesRGB
{
    internal class LoopbackAudio : IAudioProvider
    {
        private WasapiLoopbackCapture _audio;
        private BufferedWaveProvider _bwp;
        private DateTime? _lastSilence;
        private static readonly int _bUFFER_SIZE = (int) Math.Pow(2, 13);
        private static readonly TimeSpan _mAX_SILENCE = TimeSpan.FromSeconds(10);

        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        public event EventHandler<float[]> OnAudioData;

        private void Calculate()
        {
            if (_bwp.BufferedBytes < _bUFFER_SIZE)
                return;
            var buffer = new byte[_bUFFER_SIZE];
            _bwp.Read(buffer, 0, _bUFFER_SIZE);
            var leftSamples = new float[_bUFFER_SIZE / 8];
            var rightSamples = new float[_bUFFER_SIZE / 8];
            var joinedSamples = new float[_bUFFER_SIZE / 8];

            for (var i = 0; i < _bUFFER_SIZE; i += 8)
            {
                leftSamples[i / 8] = BitConverter.ToSingle(buffer, i);
                rightSamples[i / 8] = BitConverter.ToSingle(buffer, i + 4);
                joinedSamples[i / 8] = (leftSamples[i / 8] + rightSamples[i / 8]) / 2;
            }

            OnAudioData?.Invoke(this, FFT(joinedSamples));
        }

        private float[] FFT(float[] data)
        {
            var fft = new float[data.Length];
            var fftComplex = new Complex[data.Length];
            for (var i = 0; i < data.Length; i++)
                fftComplex[i] = new Complex(data[i], 0.0);
            Accord.Math.Transforms.FourierTransform2.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Backward);
            for (var i = 0; i < data.Length; i++)
                fft[i] = (float) fftComplex[i].Magnitude;
            return fft;
        }

        public void Dispose() =>
            Task.WhenAny(Task.Run(() => _audio?.Dispose()), Task.Delay(10000)).Wait();

        public Task Start()
        {
            _audio = new WasapiLoopbackCapture();
            _bwp = new BufferedWaveProvider(_audio.WaveFormat) { DiscardOnBufferOverflow = true, BufferLength = _bUFFER_SIZE * 2 };
            _audio.DataAvailable += (sndr, args) =>
            {
                _bwp.AddSamples(args.Buffer, 0, args.BytesRecorded);
                Calculate();
                if (args.BytesRecorded == 0)
                {
                    if (_lastSilence == null)
                        _lastSilence = DateTime.Now;
                    else if (DateTime.Now - _lastSilence > _mAX_SILENCE)
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
