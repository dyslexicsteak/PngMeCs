namespace PngMeCs.Format;

public struct ChunkType : IEquatable<ChunkType>
{
    private byte[] _type;
    public byte[] Type
    {
        readonly get => _type;
        set
        {
            if (value.Length != 4)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Type must be 4 bytes long.");
            }

            if (!value.All(b => b is (>= 65 and <= 90) or (>= 97 and <= 122)))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Type must contain only A-Z or a-z characters.");
            }

            _type = value;
        }
    }

    public readonly bool IsAncillary() => (_type[0] & 0b0010_0000) != 0;
    public readonly bool IsCritical() => !IsAncillary();

    public readonly bool IsPublic() => (_type[1] & 0b0010_0000) != 0;
    public readonly bool IsPrivate() => !IsPublic();

    public readonly bool IsReservedBitInvalid() => (_type[2] & 0b0010_0000) != 0;
    public readonly bool IsReservedBitValid() => !IsReservedBitInvalid();

    public readonly bool IsSafeToCopy() => (_type[3] & 0b0010_0000) != 0;
    public readonly bool IsUnsafeToCopy() => !IsSafeToCopy();

    public readonly bool IsValid() => _type.All(b => b is (>= 65 and <= 90) or (>= 97 and <= 122));

    public ChunkType(byte[] type)
    {
        if (type.Length != 4)
        {
            throw new ArgumentOutOfRangeException(nameof(type), "ChunkType must be 4 bytes long.");
        }

        if (!type.All(b => b is (>= 65 and <= 90) or (>= 97 and <= 122)))
        {
            throw new ArgumentOutOfRangeException(nameof(type), "ChunkType must contain only A-Z or a-z characters.");
        }

        _type = type;
    }

    public static explicit operator ChunkType(byte[] type) => new(type);
    public static implicit operator byte[](ChunkType type) => type.Type;

    public override readonly string ToString() => System.Text.Encoding.UTF8.GetString(Type);

    public static bool operator ==(ChunkType left, ChunkType right) => left.Equals(right);
    public static bool operator !=(ChunkType left, ChunkType right) => !(left == right);
    public override readonly bool Equals(object? obj) => obj is ChunkType type && ((IEquatable<ChunkType>)this).Equals(type);
    readonly bool IEquatable<ChunkType>.Equals(ChunkType other) => Type.SequenceEqual(other.Type);
    public override readonly int GetHashCode() => HashCode.Combine(_type);
}
