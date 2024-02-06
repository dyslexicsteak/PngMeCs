using Cli;
using CommandLine;
using PngMeCs.Format;
using System.Text;

_ = Parser.Default.ParseArguments<EncodeSubOptions, PrintSubOptions, DecodeSubOptions>(args)
.WithParsed<EncodeSubOptions>(Encode)
.WithParsed<PrintSubOptions>(Print)
.WithParsed<DecodeSubOptions>(Decode)
.WithNotParsed(_options => Console.WriteLine("Could not parse arguments."));

void Encode(EncodeSubOptions options)
{
    bool exists = File.Exists(options.Path!);
    Console.WriteLine($"Encoding {options.ChunkType} with data {options.Data} and appending to {options.Path}");
    // perform the action

    ChunkType chunkType = new(Encoding.ASCII.GetBytes(options.ChunkType!));
    Chunk chunk = new((uint)options.Data!.Length, chunkType, Encoding.ASCII.GetBytes(options.Data));

    using var fileStream = File.OpenWrite(options.Path!);

    if (!exists)
    {
        // Write the PNG signature if the file is new
        fileStream.Write(Png.Signature);
    }

    // Append the chunk to the file
    _ = fileStream.Seek(0, SeekOrigin.End);
    fileStream.Write((byte[])chunk);

    Console.WriteLine("Done.");
}

void Print(PrintSubOptions options)
{
    Console.WriteLine($"Printing {options.ChunkType} from {options.Path}");
    using var fileStream = File.OpenRead(options.Path!);
    byte[] buf = new byte[fileStream.Length];
    _ = fileStream.Read(buf);

    Png png = new(buf);

    foreach (Chunk chunk in png.GetChunksByType(new(Encoding.ASCII.GetBytes(options.ChunkType!))))
    {
        Console.WriteLine($"Chunk: {chunk.Type} - {chunk.DataAsString()}");
    }
}

void Decode(DecodeSubOptions options)
{
    Console.WriteLine($"Decoding {options.ChunkType} from {options.Path}");
    using var fileStream = File.OpenRead(options.Path!);
    byte[] buf = new byte[fileStream.Length];
    _ = fileStream.Read(buf);

    Png png = new(buf);

    foreach (Chunk chunk in png.GetChunksByType(new(Encoding.ASCII.GetBytes(options.ChunkType!))))
    {
        Console.WriteLine(chunk);
    }
}
