using System.Text;
using PngMeCs.Format;

namespace Tests;

[TestClass]
public class ChunkTests
{
    public TestContext? TestContext { get; set; }
    private static Chunk? _chunk;

    [ClassInitialize]
    public static void ClassInitialize(TestContext context)
    {
        int dataLength = 42;
        byte[] chunkType = Encoding.ASCII.GetBytes("RuSt");
        byte[] messageBytes = Encoding.ASCII.GetBytes("This is where your secret message will be!");
        uint crc = 2882656334;

        byte[] chunkData = [
            .. BitConverter.GetBytes(dataLength),
            .. chunkType,
            .. messageBytes,
            .. BitConverter.GetBytes(crc)
        ];

        _chunk = new(chunkData);
    }

    [TestCategory("Construction"), TestMethod("Test CRC Computation")]
    public void TestCrcComputation()
    {
        ChunkType chunkType = new(Encoding.ASCII.GetBytes("RuSt"));
        byte[] messageBytes = Encoding.ASCII.GetBytes("This is where your secret message will be!");
        uint dataLength = (uint)messageBytes.Length;

        Chunk chunk = new(dataLength, chunkType, messageBytes);
        uint crc = chunk.Crc;

        Assert.AreEqual(_chunk!.Crc, crc);
    }

    [TestCategory("Construction"), TestMethod("Test Invalid Construction")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestInvalidConstruction()
    {
        byte[] bytes = new byte[12];
        Chunk chunk = new(bytes);

        Assert.AreEqual<uint>(0, chunk.Length);
        Assert.AreEqual(new ChunkType(new byte[4]), chunk.Type);
        Assert.AreEqual(Array.Empty<byte>(), chunk.Data);
        Assert.AreEqual<uint>(0, chunk.Crc);
    }

    [TestCategory("Member"), TestMethod("Test Data As String")]
    public void TestDataAsString()
    {
        Assert.AreEqual("This is where your secret message will be!", _chunk!.DataAsString());
    }

    [TestCategory("Member"), TestMethod("Test Length")]
    public void TestLength()
    {
        Assert.AreEqual<uint>(42, _chunk!.Length);
    }

    [TestCategory("Member"), TestMethod("Test Type")]
    public void TestType()
    {
        Assert.AreEqual("RuSt", _chunk!.Type.ToString());
    }

    [TestCategory("Member"), TestMethod("Test CRC")]
    public void TestCrc()
    {
        Assert.AreEqual(2882656334, _chunk!.Crc);
    }

    [TestCategory("Operator"), TestMethod("Test Explicit Conversion")]
    public void TestExplicitConversion()
    {
        byte[] bytes = (byte[])_chunk!;
        (uint length, ChunkType type, byte[] data, uint crc) = _chunk!;

        // Big endian support is this apparently...
        Assert.AreEqual(length, BitConverter.ToUInt32(
            bytes.AsSpan()[..4].ToArray().Reverse().ToArray())
        );
        Assert.AreEqual(type, new ChunkType(bytes[4..8]));
        CollectionAssert.AreEqual(data, bytes[8..^4]);
        Assert.AreEqual(crc, BitConverter.ToUInt32(bytes.AsSpan()[^4..]));
    }
}