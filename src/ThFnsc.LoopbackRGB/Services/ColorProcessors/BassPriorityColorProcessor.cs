using ThFnsc.LoopbackRGB.Models;

namespace ThFnsc.LoopbackRGB.Services.ColorProcessors;
public class BassPriorityColorProcessor : IColorProcessor
{
    public float Order { get; } = 0;

    public RGBColor Process(RGBColor input)
    {
        var attenuation = 1 - (input.Red / (float)byte.MaxValue);
        return new(
            input.Red,
            (byte)(input.Green * attenuation),
            (byte)(input.Blue * attenuation));
    }
}
