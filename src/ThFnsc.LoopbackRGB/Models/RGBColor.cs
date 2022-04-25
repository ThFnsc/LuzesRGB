namespace ThFnsc.LoopbackRGB.Models;

public readonly struct RGBColor
{
    public readonly byte Red;
    public readonly byte Green;
    public readonly byte Blue;

    public RGBColor(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
}