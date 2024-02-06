using System.Text;
using PngMeCs.Format;

namespace Tests;

[TestClass]
public class ChunkTypeTests
{
    public TestContext? TestContext { get; set; }
    private static ChunkType _chunkType = new(Encoding.ASCII.GetBytes("RuSt"));

    [TestCategory("Construction"), TestMethod("Test Invalid Length")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestInvalidLength()
    {
        byte[] bytes = new byte[3];
        _ = new ChunkType(bytes);
    }

    [TestCategory("Construction"), TestMethod("Test Invalid Data")]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TestInvalidData()
    {
        byte[] bytes = [0, 0, 0, 0];
        ChunkType chunkType = new(bytes);
    }

    [TestCategory("Member"), TestMethod("Test Is Ancillary")]
    public void TestIsAncillary()
    {
        Assert.IsFalse(_chunkType.IsAncillary());
    }

    [TestCategory("Member"), TestMethod("Test Is Critical")]
    public void TestIsCritical()
    {
        Assert.IsTrue(_chunkType.IsCritical());
    }

    [TestCategory("Member"), TestMethod("Test Is Private")]
    public void TestIsPrivate()
    {
        Assert.IsFalse(_chunkType.IsPrivate());
    }

    [TestCategory("Member"), TestMethod("Test Is Public")]
    public void TestIsPublic()
    {
        Assert.IsTrue(_chunkType.IsPublic());
    }

    [TestCategory("Member"), TestMethod("Test Is Reserved Bit Valid")]
    public void TestIsReservedBitValid()
    {
        Assert.IsTrue(_chunkType.IsReservedBitValid());
    }

    [TestCategory("Member"), TestMethod("Test Is Reserved Bit Invalid")]
    public void TestIsReservedBitInvalid()
    {
        Assert.IsFalse(_chunkType.IsReservedBitInvalid());
    }

    [TestCategory("Member"), TestMethod("Test Is Unsafe To Copy")]
    public void TestIsUnsafeToCopy()
    {
        Assert.IsFalse(_chunkType.IsUnsafeToCopy());
    }

    [TestCategory("Member"), TestMethod("Test Is Safe To Copy")]
    public void TestIsSafeToCopy()
    {
        Assert.IsTrue(_chunkType.IsSafeToCopy());
    }

    [TestCategory("Member"), TestMethod("Test Is Valid")]
    public void TestIsValid()
    {
        Assert.IsTrue(_chunkType.IsValid());
    }

    [TestCategory("Member"), TestMethod("Test ToString")]
    public void TestToString()
    {
        Assert.AreEqual("RuSt", _chunkType.ToString());
    }

    [TestCategory("Operator"), TestMethod("Test Equality")]
    public void TestEquality()
    {
        ChunkType chunkType = new(Encoding.ASCII.GetBytes("RuSt"));
        Assert.IsTrue(_chunkType == chunkType);
    }

    [TestCategory("Operator"), TestMethod("Test Inequality")]
    public void TestInequality()
    {
        ChunkType chunkType = new(Encoding.ASCII.GetBytes("RuSt"));
        Assert.IsFalse(_chunkType != chunkType);
    }

    [TestCategory("Operator"), TestMethod("Test Implicit Conversion")]
    public void TestImplicitConversion()
    {
        byte[] bytes = _chunkType;
        CollectionAssert.AreEqual(Encoding.ASCII.GetBytes("RuSt"), bytes);
    }

    [TestCategory("Operator"), TestMethod("Test Explicit Conversion")]
    public void TestExplicitConversion()
    {
        ChunkType chunkType = (ChunkType)Encoding.ASCII.GetBytes("RuSt");
        Assert.AreEqual(_chunkType, chunkType);
    }

}