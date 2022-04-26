using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.Devices;
public interface IColoreableDevice
{
    void SetColors(RGBColor[] colors);
}
