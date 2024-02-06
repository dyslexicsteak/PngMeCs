using CommandLine;

namespace Cli;

[Verb("decode", HelpText = "Decode a chunk from a PNG file.")]
public class DecodeSubOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the PNG file.")]
    public string? Path { get; set; }

    [Option('t', "type", Required = true, HelpText = "Chunk type to decode.")]
    public string? ChunkType { get; set; }
}
