using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Drawing
{
    public static class ColorExtensions
    {
        public static Color SetMaxBrightness(this Color input, byte max) =>
            Color.FromArgb(input.R.Map(0, 255, 0, max), input.G.Map(0, 255, 0, max), input.B.Map(0, 255, 0, max));
    }
}
