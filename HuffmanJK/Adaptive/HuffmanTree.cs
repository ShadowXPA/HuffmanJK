namespace HuffmanJK.Adaptive;

public class HuffmanTree
{
    public const int HMAX = 256;
    public const int NYT = HMAX;
    public const int INTERNAL_NODE = HMAX + 1;

    public HuffmanNode Root { get; set; }
    public HuffmanNode ListHead { get; set; }
    public HuffmanNode ListTail { get; set; }
    public HuffmanNode[] Loc { get; private set; } = new HuffmanNode[HMAX + 1];

    public HuffmanTree()
    {
        Root = ListHead = ListTail = Loc[NYT] = new();
        Root.Symbol = (char)NYT;
    }
}
