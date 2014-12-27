using System;
using NUnit;
using NUnit.Framework;
using CodeInk.SWIFT;
using CodeInk.SWIFT.Message;

namespace CodeInk.SWIFT.Test
{
    [TestFixture]
    public class SWIFTMessageTest
    {
        [Test]
        public void SWIFTBasicBlock_BuildMessage_TestBasicBlockProperties()
        {
            BasicBlock basicblock = new BasicBlock();
            basicblock.BuildBlock("{1:F01BANKUSAXXXXX1234567890}");
            // message application type is F i.e.; fin message
            Assert.AreEqual("F", basicblock.ApplicationID);
            // Service identifier two characters
            Assert.AreEqual("01", basicblock.ServiceID);
            // Terminal address 12 characters
            Assert.AreEqual("BANKUSAXXXXX", basicblock.TerminalAddress);
            // session number 4 characters
            Assert.AreEqual("1234", basicblock.SessionNumber);
            // sequennce number 6 characters
            Assert.AreEqual("567890", basicblock.SequenceNumber);
        }

        [Test]
        public void SWIFTBodyBlock_BuildMessage_TestAppBlockInputProperties()
        {
            ApplicationBlock appblock = new ApplicationBlock();
            appblock.BuildBlock("{2:I103BANKDEFFXXXXU3003}");

            Assert.AreEqual("I", appblock.IOType);
            Assert.AreEqual("103", appblock.MessageType);
            Assert.AreEqual("U", appblock.Priority);

            Assert.AreEqual("BANKDEFFXXXX", appblock.ReceiverAddress);
            Assert.AreEqual("3", appblock.DeliveryMonitoing);
            Assert.AreEqual("003", appblock.ObsolescencePeriod);
        }

        [Test]
        public void SWIFTBodyBlock_BuildMessage_TestAppBlockOutputProperties()
        {
            ApplicationBlock appblock = new ApplicationBlock();
            appblock.BuildBlock("{2:O1001200970103BANKBEBBAXXX22221234569701031201N}");

            Assert.AreEqual("O", appblock.IOType);
            Assert.AreEqual("100", appblock.MessageType);
            Assert.AreEqual("N", appblock.Priority);

            Assert.AreEqual("1200", appblock.InputTime);
            Assert.AreEqual("970103BANKBEBBAXXX2222123456", appblock.MessageInputReference);
            Assert.AreEqual("970103", appblock.OutputDate);
            Assert.AreEqual("1201", appblock.OutputTime);
        }

        [Test]
        public void SWIFTUserBlock_BuildMessage_TestPropertyRetrival()
        {
            UserBlock userBlock = new UserBlock();
            userBlock.BuildBlock("{3:{113:xxxx}{108:abcdefgh12345678}}");

            Assert.AreEqual("abcdefgh12345678", userBlock.GetTagValue("108"));
            Assert.AreEqual("xxxx", userBlock.GetTagValue("113"));
            Assert.IsNull(userBlock.GetTagValue("104"));
        }

        [Test]
        public void SWIFTTailerBlock_BuildMessage_TestPropertyRetrival()
        {
            TailerBlock tailerBlock = new TailerBlock();
            tailerBlock.BuildBlock("{5:{MAC:12345678}{CHK:123456789ABC}}");

            Assert.AreEqual("12345678", tailerBlock.GetTagValue("MAC"));
            Assert.AreEqual("123456789ABC", tailerBlock.GetTagValue("CHK"));
            Assert.IsNull(tailerBlock.GetTagValue("PDE"));
        }
    }
}
