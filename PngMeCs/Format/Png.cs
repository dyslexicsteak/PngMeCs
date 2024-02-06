namespace PngMeCs.Format;

public class Png
{
    public static readonly byte[] Signature = [137, 80, 78, 71, 13, 10, 26, 10];

    public List<Chunk> Chunks { get; }

    public Png(IEnumerable<Chunk> chunks)
    {
        Chunks = new(chunks);
    }

    public Png(byte[] bytes)
    {
        int index = 0;
        Span<byte> signature = bytes.AsSpan()[..8];

        if (!signature.SequenceEqual(Signature))
        {
            throw new ArgumentOutOfRangeException(nameof(bytes), "Invalid PNG signature");
        }

        index += 8;

        Chunks = new List<Chunk>(bytes.Length / 12);

        while (index < bytes.Length)
            checked
            {
                // Big endian support is this apparently...
                uint length = BitConverter.ToUInt32(bytes[index..(index + 4)].Reverse().ToArray(), 0);
                index += 4;

                byte[] chunkType = bytes[index..(index + 4)];
                index += 4;

                int signedLength = (int)length;

                byte[] chunkData = bytes[index..(index + signedLength)];
                index += signedLength;

                uint crc = BitConverter.ToUInt32(bytes, index);
                index += 4;

                Chunk chunk = new(length, new(chunkType), chunkData, crc);
                Chunks.Add(chunk);
            }
    }

    public void AppendChunk(Chunk chunk)
    {
        Chunks.Add(chunk);
    }

    public Chunk? RemoveChunk(ChunkType type)
    {
        Chunk? chunk = Chunks.FirstOrDefault(c => c.Type == type);

        if (chunk is not null)
        {
            if (!Chunks.Remove(chunk))
            {
                throw new InvalidOperationException("Failed to remove chunk.");
            }
        }

        return chunk;
    }

    public Span<Chunk> GetChunksByType(ChunkType type) => Chunks.Where(c => c.Type.Equals(type)).ToArray();


    public byte[] ToBytes()
    {
        List<byte> bytes = new(Chunks.Count * 12 + 8);
        bytes.AddRange(Signature);

        foreach (Chunk chunk in Chunks)
        {
            bytes.AddRange((byte[])chunk);
        }

        return [.. bytes];
    }

    public static explicit operator byte[](Png png) => png.ToBytes();

    public override string ToString() => string.Join(Environment.NewLine, Chunks);
}
