using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataDecommision;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestDecode1()
        {
            //0112345678912345
            //10123456
            //17331012
            //21123457
            ScannerDecoder.ContinousDecodeString("010030054017713321100442417293211724043010AB8366A");
            //Assert.AreEqual(("00300540177133", "AB8366A", "240430", "10044241729321"), value);
        }

        [TestMethod]
        public void TestDecode2()
        {
            
            //0112345678912345
            //10123456
            //17331012
            //21123457
            var value = ScannerDecoder.DecodeString("011234567891234521123457101234561733101230");
            Assert.AreEqual(("12345678912345", "123456", "331012", "123457"), value);
        }

        [TestMethod]
        public void TestDecode3()
        {
            //0112345678912345
            //10123456
            //17331012
            //21123457
            var value = ScannerDecoder.DecodeString("01123456789123451012345617331012");
            Assert.AreEqual(("12345678912345", "123456", "331012", null), value);
        }

        [TestMethod]
        public void TestDecode4()
        {
            //0112345678912345
            //10123456
            //17331012
            //21123457
            var value = ScannerDecoder.DecodeString("01123456789123452112345717331012");
            Assert.AreEqual(("12345678912345", null, "331012", "123457"), value);
        }

        [TestMethod]
        public void TestDecode5()
        {
            //0112345678912345
            //10123456
            //17331012
            //21123456
            var value = ScannerDecoder.DecodeString("0112345678912345173310122112345717331012");
            Assert.AreEqual(("12345678912345", null, "331012", "12345717331012"), value);
        }
    }
}
