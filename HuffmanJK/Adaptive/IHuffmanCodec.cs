namespace HuffmanJK.Adaptive;

public interface IHuffmanCodec
{
    byte[] Encode(string text);
    string Decode(byte[] buffer);
}
