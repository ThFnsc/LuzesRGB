namespace ThFnsc.LoopbackRGB.Services.Devices;

public class BinaryProtocolException : Exception
{
    public BinaryProtocolException(ProtocolError error)
    {
        Error = error;
    }

    public ProtocolError Error { get; }
}