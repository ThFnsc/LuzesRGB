using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LuzesRGB.Services
{
    public interface IColorizable
    {
        Task SetColor(Color color);
        Task<Color> GetColor();

        event EventHandler<Color> OnColorChanged;
    }
}
