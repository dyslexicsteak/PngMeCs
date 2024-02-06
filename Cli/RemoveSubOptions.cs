using CommandLine;

namespace Cli;

[Verb("remove", HelpText = "Remove a chunk from a PNG file.")]
public class RemoveSubOptions
{
    [Option('p', "path", Required = true, HelpText = "Path to the PNG file.")]
    public string? Path { get; set; }

    [Option('t', "type", Required = true, HelpText = "Chunk type to remove.")]
    public string? ChunkType { get; set; }
}
