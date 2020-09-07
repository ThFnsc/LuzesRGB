using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
