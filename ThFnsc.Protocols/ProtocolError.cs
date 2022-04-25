namespace ThFnsc.LoopbackRGB.Services.Devices;

public enum ProtocolError
{
    InvalidStartByte,
    InvalidEscapeByte,
    UnexpectedSpecialByte,
    InvalidChecksum,
    MessageNotEnded,
    UnexpectedEnd,
    InvalidMessageSize,
    MessageLongerThanExpected
}
