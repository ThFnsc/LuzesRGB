class EscapedBinaryProtocol
{
private:
    bool reading = false;
    int readIndex = 0;
    int expectedSize = -1;
    bool escapeNext = false;

public:
    static const byte Start = 0xFF;
    static const byte End = 0x00;
    static const byte Escape = 0x80;
    static const byte EscapedStart = 0x81;
    static const byte EscapedEnd = 0x83;
    static const byte EscapedEscape = 0x84;

    byte *buffer;
    int bufferSize;

    EscapedBinaryProtocol(int bufferSize)
    {
        this->bufferSize = bufferSize;
        buffer = new byte[bufferSize];
    }

    static void Write(byte *input, int length)
    {
        Serial.write(Start);

        if (length > 127)
            throw "This protocol does not support messages longer than 127 bytes yet.";

        EscapeByte((byte)length);

        for (int i = 0; i < length; i++)
            EscapeByte(input[i]);

        Serial.write(End);
    }

    int Read()
    {
        while (1)
        {
            int b = Serial.read();
            if (b == -1)
                return -1; // No data available
            if (!reading)
            {
                if (b == Start) // Discard otherwise
                    reading = true;
            }
            else
            {
                int read = UnescapeByte(b);
                switch (read)
                {
                case -1: // Current byte is escape byte
                    continue;
                case -2: // Current byte is end byte
                    if (expectedSize == readIndex)
                    {
                        int size = expectedSize;
                        ResetReader();
                        return size;
                    }
                    else
                    {
                        ResetReader();
                        return -5;
                    }
                case -3: // Start byte not expected
                case -4: // Invalid escape byte
                    ResetReader();
                    return read;
                default:
                    if (expectedSize == -1)
                    {
                        expectedSize = read;
                        continue;
                    }
                    if (readIndex >= expectedSize)
                    {
                        ResetReader();
                        return -2;
                    }
                    buffer[readIndex++] = read;
                }
            }
        }
    }

private:
    void ResetReader()
    {
        reading = false;
        readIndex = 0;
        expectedSize = -1;
        escapeNext = false;
    }

    static void EscapeByte(byte bToWrite)
    {
        switch (bToWrite)
        {
        case Start:
            Serial.write(Escape);
            Serial.write(EscapedStart);
            break;
        case End:
            Serial.write(Escape);
            Serial.write(EscapedEnd);
            break;
        case Escape:
            Serial.write(Escape);
            Serial.write(EscapedEscape);
            break;
        default:
            Serial.write(bToWrite);
            break;
        }
    }

    int UnescapeByte(byte b)
    {
        if (escapeNext)
        {
            escapeNext = false;
            switch (b)
            {
            case EscapedStart:
                return Start;
            case EscapedEnd:
                return End;
            case EscapedEscape:
                return Escape;
            default:
                return -4; // Invalid escape byte
            }
        }
        else
        {
            switch (b)
            {
            case Start:
                return -3; // Start byte not expected
            case Escape:
                escapeNext = true;
                return -1;
            case End:
                return -2;
            default:
                return b;
            }
        }
    }
};