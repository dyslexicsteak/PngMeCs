using CommandLine;

namespace Cli;

[Verb("encode", HelpText = "Encode a chunk and append it to a PNG file.")]
public class EncodeSubOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the PNG file.")]
    public string? Path { get; set; }

    [Option('t', "type", Required = true, HelpText = "Chunk type to encode.")]
    public string? ChunkType { get; set; }

    [Option('d', "data", Required = true, HelpText = "Data to encode.")]
    public string? Data { get; set; }

    [Option('o', "output", Required = false, HelpText = "Output file.")]
    public string? Output { get; set; }
}
