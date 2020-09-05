using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB
{
    interface IAudioProvider : IDisposable
    {
        void Start();
        void Stop();
        event EventHandler<float[]> OnAudioData;
    }
}
