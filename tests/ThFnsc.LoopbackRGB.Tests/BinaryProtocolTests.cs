using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography;
using ThFnsc.LoopbackRGB.Services.Devices;

namespace ThFnsc.LoopbackSerialRGB.Tests;
[TestClass]
public class BinaryProtocolTests
{
    [TestMethod]
    public void TestSerialization()
    {
        for(var c = 0; c < 100; c++)
        {
            var bytes = GetRandomBytes(120);
            var escaped = EscapedBinaryProtocol.Write(bytes);

            Assert.AreEqual(escaped[0], EscapedBinaryProtocol.Start);
            Assert.AreEqual(escaped[^1], EscapedBinaryProtocol.End);
            for (var i = 2; i < escaped.Length - 1; i++)
            {
                Assert.AreNotEqual(escaped[i], EscapedBinaryProtocol.Start);
                Assert.AreNotEqual(escaped[i], EscapedBinaryProtocol.End);
            }

            var unescaped = EscapedBinaryProtocol.Read(escaped);
            CollectionAssert.AreEqual(bytes, unescaped);
        }
    }

    [TestMethod]
    public void TestByteAmounts()
    {
        var rnd = new Random();
        for(var i = 0; i < 100; i++)
        {
            var nbr = rnd.Next(256);
            var asBin = Convert.ToString(nbr, 2);
            Console.WriteLine($"{nbr} {asBin}({asBin.Length}) {EscapedBinaryProtocol.HowManyBitsForTheNumber(nbr)}");
        }
    }
    
    private byte[] GetRandomBytes(int count) =>
        RandomNumberGenerator.GetBytes(count);
}