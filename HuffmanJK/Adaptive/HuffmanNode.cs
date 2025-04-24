namespace HuffmanJK.Adaptive;

public class HuffmanNode
{
    public HuffmanNode? Parent { get; set; }
    public HuffmanNode? Left { get; set; }
    public HuffmanNode? Right { get; set; }
    public HuffmanNode? Previous { get; set; }
    public HuffmanNode? Next { get; set; }
    public HuffmanNodeHead? Head { get; set; }
    public char? Symbol { get; set; }
    public int Weight { get; set; }
}
