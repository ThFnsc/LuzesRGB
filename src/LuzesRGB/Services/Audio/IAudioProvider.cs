using System;
using System.Threading.Tasks;

namespace LuzesRGB
{
    public interface IAudioProvider : IDisposable
    {
        Task Start();
        Task Stop();
        event EventHandler<float[]> OnAudioData;
    }
}
