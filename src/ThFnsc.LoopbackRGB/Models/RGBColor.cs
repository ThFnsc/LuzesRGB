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

    public static RGBColor FromByteArray(byte[] bytes)
    {
        if (bytes.Length != 3)
            throw new ArgumentException("Byte array must be of length 3");

        return new RGBColor(bytes[0], bytes[1], bytes[2]);
    }

    public byte[] ToByteArray() => new byte[] { Red, Green, Blue };

    public override string ToString() => $"#{Convert.ToHexString(ToByteArray())}";
}