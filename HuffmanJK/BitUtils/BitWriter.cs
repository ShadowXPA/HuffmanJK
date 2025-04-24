namespace HuffmanJK.BitUtils;

public class BitWriter
{
    private List<byte> buffer = new();
    private int bitPos = 0;
    private byte currentByte = 0;

    public void WriteBit(int bit)
    {
        if (bit != 0) currentByte |= (byte)(bit << (bitPos & 7));

        bitPos++;

        if (bitPos == 8)
        {
            buffer.Add(currentByte);
            bitPos = 0;
            currentByte = 0;
        }
    }

    public void WriteByte(byte b)
    {
        for (int i = 0; i < 8; i++)
        {
            WriteBit((b >> i) & 1);
        }
    }

    public byte[] Finish()
    {
        if (bitPos > 0) buffer.Add(currentByte);
        return [.. buffer];
    }
}
