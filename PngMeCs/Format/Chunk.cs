namespace PngMeCs.Format;

public class Chunk
{
    public Chunk(byte[] bytes)
    {
        if (bytes.Length < 12)
        {
            throw new ArgumentOutOfRangeException(nameof(bytes), "Chunk must be at least 12 bytes long.");
        }

        Length = BitConverter.ToUInt32(bytes.AsSpan()[..4]);
        Type = new ChunkType(bytes[4..8]);
        Data = bytes[8..^4];
        Crc = BitConverter.ToUInt32(bytes.AsSpan()[^4..]);
    }

    public Chunk(uint length, ChunkType type, byte[] data)
    {
        Length = length;
        Type = type;
        Data = data;
        Crc = BitConverter.ToUInt32(System.IO.Hashing.Crc32.Hash(type.Type.Concat(data).ToArray()));
    }

    public Chunk(uint length, ChunkType type, byte[] data, uint crc)
    {
        Length = length;
        Type = type;
        Data = data;
        Crc = crc;
    }

    public static explicit operator byte[](Chunk chunk)
    {
        byte[] bytes = new byte[12 + chunk.Length];

        // Big endian support is this apparently...
        if (
            !BitConverter.TryWriteBytes(
                bytes.AsSpan()[..4],
                BitConverter.ToUInt32(BitConverter.GetBytes(chunk.Length).Reverse().ToArray())
            )
        )
        {
            throw new InvalidOperationException("Failed to write chunk length.");
        }

        chunk.Type.Type.CopyTo(bytes, 4);
        chunk.Data.CopyTo(bytes, 8);

        return !BitConverter.TryWriteBytes(bytes.AsSpan()[^4..], chunk.Crc)
            ? throw new InvalidOperationException("Failed to write chunk CRC.")
            : bytes;
    }

    public string DataAsString() => System.Text.Encoding.UTF8.GetString(Data);

    public void Deconstruct(out uint length, out ChunkType type, out byte[] data, out uint crc)
    {
        length = Length;
        type = Type;
        data = Data;
        crc = Crc;
    }

    public override string ToString()
    {
        return $"Chunk: {Type},\nLength: {Length},\nCRC: {Crc},\nData: {DataAsString()}";
    }

    public uint Crc { get; set; }
    public byte[] Data { get; set; }
    public uint Length { get; set; }
    public ChunkType Type { get; set; }
}
