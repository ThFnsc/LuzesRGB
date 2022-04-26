using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.ColorProcessors;
public interface IColorProcessor
{
    RGBColor Process(RGBColor input);

    float Order { get; }
}