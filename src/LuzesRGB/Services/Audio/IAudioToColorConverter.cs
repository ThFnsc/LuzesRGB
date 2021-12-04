using System;
using System.Drawing;

namespace LuzesRGB
{
    public interface IAudioToColorConverter
    {
        void NewSpectrum(object sender, float[] spectrum);
        event EventHandler<Color> OnColorAvailable;
    }
}
