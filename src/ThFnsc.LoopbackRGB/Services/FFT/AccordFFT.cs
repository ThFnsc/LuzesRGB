using Accord.Math.Transforms;
using System.Numerics;

namespace ThFnsc.LoopbackRGB.Services.FFT;

public class AccordFFT : IFFTCalculator
{
    private Complex[] _complexArr = Array.Empty<Complex>();

    public void Calculate(float[] samples)
    {
        lock (this)
        {
            if (_complexArr.Length != samples.Length)
                Array.Resize(ref _complexArr, samples.Length);
            for (var i = 0; i < samples.Length; i++)
                _complexArr[i] = samples[i];
            FourierTransform2.FFT(_complexArr, Accord.Math.FourierTransform.Direction.Backward);
            for (var i = 0; i < samples.Length; i++)
                samples[i] = (float)_complexArr[i].Magnitude;
        }
    }
}
