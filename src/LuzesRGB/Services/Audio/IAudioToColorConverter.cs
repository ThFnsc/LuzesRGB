using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB
{
    public interface IAudioToColorConverter
    {
        void NewSpectrum(object sender, float[] spectrum);
        event EventHandler<Color> OnColorAvailable;
    }
}
