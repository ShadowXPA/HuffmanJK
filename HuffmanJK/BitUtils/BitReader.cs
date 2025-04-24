namespace HuffmanJK.BitUtils;

public class BitReader
{
    private readonly byte[] buffer;
    private int bytePos;
    private int bitPos;
    public bool EndOfStream => bytePos >= buffer.Length;

    public BitReader(byte[] data)
    {
        buffer = data;
        bytePos = 0;
        bitPos = 0;
    }

    public bool ReadBit()
    {
        if (EndOfStream) throw new EndOfStreamException();

        bool bit = (buffer[bytePos] & (1 << bitPos)) != 0;
        bitPos++;

        if (bitPos == 8)
        {
            bitPos = 0;
            bytePos++;
        }

        return bit;
    }
}
