using System.Text;
using CommandLine;
using HuffmanJK.Adaptive;

namespace HuffmanJK;

class Program
{
    [Verb("encode", HelpText = "Encodes the provided text.")]
    public class EncodeOptions
    {
        [Option("stdin", HelpText = "Read the text from standard input.", Default = false, Required = true, SetName = "stdin")]
        public bool Stdin { get; set; }

        [Option('i', "input", HelpText = "The file which contains the text to encode.", Required = true, SetName = "file")]
        public string? InputFile { get; set; }

        [Option('t', "text", HelpText = "Base64 encoded text to encode.", Required = true, SetName = "text")]
        public string? Text { get; set; }

        [Option('o', "output", HelpText = "The file which will contain the encoded output. (Will output to standard output if not provided)")]
        public string? OutputFile { get; set; }

        [Option('O', "offset", HelpText = "The text offset for encoding.", Default = 0)]
        public int Offset { get; set; }
    }

    [Verb("decode", HelpText = "Decodes the provided encoded text.")]
    public class DecodeOptions
    {
        [Option("stdin", HelpText = "Read the encoded text from standard input.", Default = false, Required = true, SetName = "stdin")]
        public bool Stdin { get; set; }

        [Option('i', "input", HelpText = "The file which contains the text to decode.", Default = null, Required = true, SetName = "file")]
        public string? InputFile { get; set; }

        [Option('t', "text", HelpText = "Base64 encoded text to decode.", Default = null, Required = true, SetName = "text")]
        public string? Text { get; set; }

        [Option('o', "output", HelpText = "The file which will contain the decoded output. (Will output to standard output if not provided)")]
        public string? OutputFile { get; set; }

        [Option('O', "offset", HelpText = "The text offset for decoding.", Default = 0)]
        public int Offset { get; set; }
    }

    static int Main(string[] args)
    {
        return Parser.Default.ParseArguments<EncodeOptions, DecodeOptions>(args)
            .MapResult(
                (EncodeOptions opts) => EncodeInput(opts),
                (DecodeOptions opts) => DecodeInput(opts),
                errors => 1
            );
    }

    private static int EncodeInput(EncodeOptions options)
    {
        string? decodedText = null;

        if (options.Stdin)
        {
            decodedText = Console.In.ReadToEnd();
        }

        if (options.InputFile is not null)
        {
            decodedText = File.ReadAllText(options.InputFile);
        }

        if (options.Text is not null)
        {
            decodedText = Encoding.UTF8.GetString(Convert.FromBase64String(options.Text));
        }

        HuffmanCodec huffmanCodec = new();
        var encodedText = huffmanCodec.Encode(decodedText![options.Offset..]);

        if (options.OutputFile is not null)
        {
            File.WriteAllBytes(options.OutputFile, encodedText);
        }
        else
        {
            using var stdout = Console.OpenStandardOutput();
            stdout.Write(encodedText, 0, encodedText.Length);
        }

        return 0;
    }

    private static int DecodeInput(DecodeOptions options)
    {
        byte[]? encodedText = null;

        if (options.Stdin)
        {
            using var stdin = Console.OpenStandardInput();
            using var memoryStream = new MemoryStream();

            stdin.CopyTo(memoryStream);
            encodedText = memoryStream.ToArray();
        }

        if (options.InputFile is not null)
        {
            encodedText = File.ReadAllBytes(options.InputFile);
        }

        if (options.Text is not null)
        {
            encodedText = Convert.FromBase64String(options.Text);
        }

        HuffmanCodec huffmanCodec = new();
        var decodedText = huffmanCodec.Decode([.. encodedText!.Skip(options.Offset)]);

        if (options.OutputFile is not null)
        {
            File.WriteAllText(options.OutputFile, decodedText);
        }
        else
        {
            Console.Write(decodedText);
        }

        return 0;
    }
}
