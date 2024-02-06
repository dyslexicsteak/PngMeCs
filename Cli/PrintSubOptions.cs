using CommandLine;

namespace Cli;

[Verb("print", HelpText = "Print a chunk from a PNG file.")]
public class PrintSubOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the PNG file.")]
    public string? Path { get; set; }

    [Option('t', "type", Required = true, HelpText = "Chunk type to print.")]
    public string? ChunkType { get; set; }
}
