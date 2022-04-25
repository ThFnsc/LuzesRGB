namespace ThFnsc.LoopbackRGB.Services.Devices;
public static class EscapedBinaryProtocol
{
    public const byte Start = 0xFF;
    public const byte End = 0x00;
    public const byte Escape = 0x80;
    public const byte EscapedStart = 0x81;
    public const byte EscapedEnd = 0x83;
    public const byte EscapedEscape = 0x84;

    public static byte[] Write(byte[] input)
    {
        var ms = new MemoryStream(input.Length);
        var writer = new BinaryWriter(ms);
        writer.Write(Start);
        
        if (input.Length > sbyte.MaxValue)
            throw new NotSupportedException("This protocol does not support messages longer than 127 bytes yet.");
        EscapeByte(writer, (byte)input.Length);
        
        for (var i = 0; i < input.Length; i++)
            EscapeByte(writer, input[i]);
            
        writer.Write(End);
        return ms.ToArray();
    }

    private static void EscapeByte(BinaryWriter writer, byte bToWrite)
    {
        switch (bToWrite)
        {
            case Start:
                writer.Write(Escape);
                writer.Write(EscapedStart);
                break;
            case End:
                writer.Write(Escape);
                writer.Write(EscapedEnd);
                break;
            case Escape:
                writer.Write(Escape);
                writer.Write(EscapedEscape);
                break;
            default:
                writer.Write(bToWrite);
                break;
        }
    }

    public static byte[] Read(byte[] input) =>
        Read(new MemoryStream(input));
    
    public static byte[] Read(Stream stream)
    {
        var reader = new BinaryReader(stream);
        if (reader.ReadByte() != Start)
            throw new BinaryProtocolException(ProtocolError.InvalidStartByte);
        var size = UnescapeByte(reader);
        if (!size.HasValue)
            throw new BinaryProtocolException(ProtocolError.UnexpectedEnd);
        else if (size > sbyte.MaxValue)
            throw new BinaryProtocolException(ProtocolError.InvalidMessageSize);
        var wms = new MemoryStream(size.Value);
        var writer = new BinaryWriter(wms);
        while (true)
        {
            var b = UnescapeByte(reader);
            if (b.HasValue)
            {
                if (--size == -1)
                    throw new BinaryProtocolException(ProtocolError.MessageLongerThanExpected);
                writer.Write(b.Value);
            }
            else
                return wms.ToArray();
        }
    }

    public static byte? UnescapeByte(BinaryReader reader)
    {
        var b = reader.ReadByte();
        switch (b)
        {
            case Start:
                throw new BinaryProtocolException(ProtocolError.UnexpectedSpecialByte);
            case Escape:
                var next = reader.ReadByte();
                switch (next)
                {
                    case EscapedStart:
                        return Start;
                    case EscapedEnd:
                        return End;
                    case EscapedEscape:
                        return Escape;
                    default:
                        throw new BinaryProtocolException(ProtocolError.InvalidEscapeByte);
                }
            case End:
                return null;
            default:
                return b;
        }
    }

    public static byte HowManyBitsForTheNumber(int number) =>
        (byte)Math.Ceiling(Math.Log(number, 2));
}
