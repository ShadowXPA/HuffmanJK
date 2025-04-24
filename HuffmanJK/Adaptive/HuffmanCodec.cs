using System.Text;
using HuffmanJK.BitUtils;

namespace HuffmanJK.Adaptive;

public class HuffmanCodec : IHuffmanCodec
{
    public byte[] Encode(string text)
    {
        HuffmanTree tree = new();
        BitWriter writer = new();
        int size = text.Length;

        writer.WriteByte((byte)(size >> 8));
        writer.WriteByte((byte)(size & 0xFF));

        foreach (char ch in text)
        {
            HuffmanTransmit(tree, ch, writer);
            HuffmanAddReference(tree, ch);
        }

        return writer.Finish();
    }

    public string Decode(byte[] buffer)
    {
        StringBuilder builder = new();
        int size = buffer.Length;
        HuffmanTree tree = new();
        BitReader reader = new([.. buffer.Skip(2)]);

        if (size <= 0)
        {
            return "";
        }

        int msgLength = buffer[0] * 256 + buffer[1];

        for (int j = 0; j < msgLength; j++)
        {
            int ch;
            ch = HuffmanReceive(tree.Root, reader);

            if (ch == HuffmanTree.NYT)
            {
                ch = 0;
                for (int i = 0; i < 8; i++)
                {
                    ch = (ch << 1) + (reader.ReadBit() ? 1 : 0);
                }
            }

            builder.Append((char)ch);

            HuffmanAddReference(tree, (char)ch);
        }

        return builder.ToString();
    }

    private void HuffmanTransmit(HuffmanTree tree, char ch, BitWriter writer)
    {
        if (tree.Loc[ch] is null)
        {
            HuffmanTransmit(tree, (char)HuffmanTree.NYT, writer);

            for (int i = 7; i >= 0; i--)
            {
                writer.WriteBit((ch >> i) & 0x1);
            }
        }
        else
        {
            HuffmanSend(tree.Loc[ch], null, writer);
        }
    }

    private void HuffmanSend(HuffmanNode node, HuffmanNode? child, BitWriter writer)
    {
        if (node.Parent is not null)
        {
            HuffmanSend(node.Parent, node, writer);
        }

        if (child is not null)
        {
            if (node.Right == child)
            {
                writer.WriteBit(1);
            }
            else
            {
                writer.WriteBit(0);
            }
        }
    }

    private char HuffmanReceive(HuffmanNode? node, BitReader reader)
    {
        while (node is not null && node.Symbol == HuffmanTree.INTERNAL_NODE)
        {
            if (reader.ReadBit())
            {
                node = node.Right;
            }
            else
            {
                node = node.Left;
            }
        }

        return node?.Symbol ?? (char)0;
    }

    private void HuffmanAddReference(HuffmanTree tree, char ch)
    {
        HuffmanNode tnode, tnode2;

        if (tree.Loc[ch] is null)
        {
            tnode = new();
            tnode2 = new();

            tnode2.Symbol = (char)HuffmanTree.INTERNAL_NODE;
            tnode2.Weight = 1;
            tnode2.Next = tree.ListHead.Next;

            if (tree.ListHead.Next is not null)
            {
                tree.ListHead.Next.Previous = tnode2;

                if (tree.ListHead.Next.Weight == 1)
                {
                    tnode2.Head = tree.ListHead.Next.Head;
                }
                else
                {
                    tnode2.Head = new();
                    tnode2.Head.Value = tnode2;
                }
            }
            else
            {
                tnode2.Head = new();
                tnode2.Head.Value = tnode2;
            }

            tree.ListHead.Next = tnode2;
            tnode2.Previous = tree.ListHead;

            tnode.Symbol = (char)ch;
            tnode.Weight = 1;
            tnode.Next = tree.ListHead.Next;

            if (tree.ListHead.Next is not null)
            {
                tree.ListHead.Next.Previous = tnode;

                if (tree.ListHead.Next.Weight == 1)
                {
                    tnode.Head = tree.ListHead.Next.Head;
                }
                else
                {
                    /* this should never happen */
                    Console.WriteLine("Well but it happened...");
                    tnode.Head = new();
                    tnode.Head.Value = tnode2;
                }
            }
            else
            {
                /* this should never happen */
                Console.WriteLine("Well but it happened...");
                tnode.Head = new();
                tnode.Head.Value = tnode;
            }

            tree.ListHead.Next = tnode;
            tnode.Previous = tree.ListHead;
            tnode.Left = tnode.Right = null;

            if (tree.ListHead.Parent is not null)
            {
                if (tree.ListHead.Parent.Left == tree.ListHead)
                {
                    tree.ListHead.Parent.Left = tnode2;
                }
                else
                {
                    tree.ListHead.Parent.Right = tnode2;
                }
            }
            else
            {
                tree.Root = tnode2;
            }

            tnode2.Right = tnode;
            tnode2.Left = tree.ListHead;

            tnode2.Parent = tree.ListHead.Parent;
            tree.ListHead.Parent = tnode.Parent = tnode2;

            tree.Loc[ch] = tnode;

            HuffmanIncrement(tree, tnode2.Parent);
        }
        else
        {
            HuffmanIncrement(tree, tree.Loc[ch]);
        }
    }

    private void HuffmanIncrement(HuffmanTree tree, HuffmanNode? node)
    {
        HuffmanNode? lnode;

        if (node is null)
        {
            return;
        }

        if (node.Next is not null && node.Next.Weight == node.Weight)
        {
            lnode = node.Head?.Value;

            if (lnode != node.Parent)
            {
                HuffmanSwap(tree, lnode!, node);
            }

            HuffmanSwapList(lnode!, node);
        }

        if (node.Previous is not null && node.Previous.Weight == node.Weight)
        {
            node.Head!.Value = node.Previous;
        }
        else
        {
            node.Head = null;
        }

        node.Weight++;

        if (node.Next is not null && node.Next.Weight == node.Weight)
        {
            node.Head = node.Next.Head;
        }
        else
        {
            node.Head = new();
            node.Head.Value = node;
        }

        if (node.Parent is not null)
        {
            HuffmanIncrement(tree, node.Parent);

            if (node.Previous == node.Parent)
            {
                HuffmanSwapList(node, node.Parent);

                if (node.Head?.Value == node)
                {
                    node.Head.Value = node.Parent;
                }
            }
        }
    }

    private void HuffmanSwap(HuffmanTree tree, HuffmanNode node1, HuffmanNode node2)
    {
        HuffmanNode? par1, par2;

        par1 = node1.Parent;
        par2 = node2.Parent;

        if (par1 is not null)
        {
            if (par1.Left == node1)
            {
                par1.Left = node2;
            }
            else
            {
                par1.Right = node2;
            }
        }
        else
        {
            tree.Root = node2!;
        }

        if (par2 is not null)
        {
            if (par2.Left == node2)
            {
                par2.Left = node1;
            }
            else
            {
                par2.Right = node1;
            }
        }
        else
        {
            tree.Root = node1;
        }

        node1.Parent = par2;
        node2.Parent = par1;
    }

    private void HuffmanSwapList(HuffmanNode node1, HuffmanNode node2)
    {
        HuffmanNode? par1;

        par1 = node1.Next;
        node1.Next = node2.Next;
        node2.Next = par1;

        par1 = node1.Previous;
        node1.Previous = node2.Previous;
        node2.Previous = par1;

        if (node1.Next == node1)
        {
            node1.Next = node2;
        }

        if (node2.Next == node2)
        {
            node2.Next = node1;
        }

        if (node1.Next is not null)
        {
            node1.Next.Previous = node1;
        }

        if (node2.Next is not null)
        {
            node2.Next.Previous = node2;
        }

        if (node1.Previous is not null)
        {
            node1.Previous.Next = node1;
        }

        if (node2.Previous is not null)
        {
            node2.Previous.Next = node2;
        }
    }
}
