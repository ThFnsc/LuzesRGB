using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.Devices;
public interface IColoreableDevice
{
    void SetColor(RGBColor color);
}
