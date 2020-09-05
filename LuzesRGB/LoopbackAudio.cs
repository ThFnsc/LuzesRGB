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
        [DllImport("shlwapi.dll")]
        public static extern int ColorHLSToRGB(int H, int L, int S);

        readonly WasapiLoopbackCapture audio;
        readonly BufferedWaveProvider bwp;
        public event EventHandler<float[]> OnAudioData;
        int samplesCaptured = 0;
        DateTime started = DateTime.MinValue;
        readonly int BUFFER_SIZE = (int)Math.Pow(2, 15);

        public LoopbackAudio()
        {
            audio = new WasapiLoopbackCapture();
            bwp = new BufferedWaveProvider(audio.WaveFormat) { DiscardOnBufferOverflow = true, BufferLength = BUFFER_SIZE * 2 };
            audio.DataAvailable += (sndr, args) => {
                bwp.AddSamples(args.Buffer, 0, args.BytesRecorded);
                Calculate();
            };
        }

        private void Calculate()
        {
            if (bwp.BufferedBytes < BUFFER_SIZE)
                return;
            samplesCaptured++;
            if (started == DateTime.MinValue)
                started = DateTime.Now;
            double perSec = samplesCaptured / (DateTime.Now - started).TotalSeconds;
            byte[] buffer = new byte[BUFFER_SIZE];
            bwp.Read(buffer, 0, BUFFER_SIZE);
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

        public void Dispose()
        {
            if (audio.CaptureState == NAudio.CoreAudioApi.CaptureState.Capturing)
                audio.StopRecording();
        }

        public void Start() =>
            audio.StartRecording();

        public void Stop() =>
            audio.StopRecording();
    }
}
