using System;
using System.Drawing;
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
