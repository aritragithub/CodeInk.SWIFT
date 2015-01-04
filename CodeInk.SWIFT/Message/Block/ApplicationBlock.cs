using System;
using System.Text.RegularExpressions;

namespace CodeInk.SWIFT.Message
{
    /// <summary>
    /// fin message application block
    /// </summary>
    public class ApplicationBlock : FinMessageBlock
    {
        const string blockId = "2";

        private const string inputAppBlockRegex = @"{2:(?<io>[I])(?<mtype>[\d]{3})(?<recadd>[\w]{12})(?<msgp>[SNU]{1})(?<deli>[123]{1})(?<obs>[\d]{3})}";
        private const string outputAppBlockRegex = @"{2:(?<io>[O])(?<mtype>[\d]{3})(?<itime>[\d]{4})(?<mir>[\w]{28})(?<odate>[\d]{6})(?<otime>[\d]{4})(?<msgp>[SNU]{1})}";

        public override string BlockID
        {
            get { return blockId; }
        }

        public override void BuildBlock(string record)
        {
            Regex bodyRegex = new Regex(inputAppBlockRegex);
            Match match = bodyRegex.Match(record);
            if (!match.Success)
            {
                bodyRegex = new Regex(outputAppBlockRegex);
                match = bodyRegex.Match(record);
                if (!match.Success)
                    throw new FormatException("invalid application block!");
            }

            IOType = match.Groups["io"].Value;
            MessageType = match.Groups["mtype"].Value;
            Priority = match.Groups["msgp"].Value;
            if (IOType == "I")
            {
                ReceiverAddress = match.Groups["recadd"].Value;
                DeliveryMonitoring = match.Groups["deli"].Value;
                ObsolescencePeriod = match.Groups["obs"].Value;
            }
            else
            {
                InputTime = match.Groups["itime"].Value;
                OutputDate = match.Groups["odate"].Value;
                OutputTime = match.Groups["otime"].Value;
                MessageInputReference = match.Groups["mir"].Value;
            }
        }

        /// <summary>
        /// Message Input/Output type
        /// </summary>
        public string IOType { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// The message priority
        /// </summary>
        public string Priority { get; set; }

        // Properties for Input message type

        /// <summary>
        /// Receiver's address with X in position 9/ It is padded with Xs if no branch is required (Input message) 
        /// </summary>
        /// <remarks>
        ///  the message priority as follows:
        ///  S = System
        ///  N = Normal
        ///  U = Urgent 
        /// </remarks>
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// Delivery monitoring field (Input message)
        /// </summary>
        /// <remarks>
        /// 1 = Non delivery warning (MT010)
        /// 2 = Delivery notification (MT011)
        /// 3 = Both valid = U1 or U3, N2 or N
        /// </remarks>
        public string DeliveryMonitoring { get; set; }

        /// <summary>
        /// Obsolescence period (Input message)
        /// </summary>
        /// <remarks>
        /// It specifies when a non-delivery notification is generated as follows:
        /// Valid for U = 003 (15 minutes)
        /// Valid for N = 020 (100 minutes)
        /// </remarks>
        public string ObsolescencePeriod { get; set; }

        // Properties for Output message type

        /// <summary>
        /// Input time with respect to the sender (Output message)
        /// </summary>
        public string InputTime { get; set; }

        /// <summary>
        /// The Message Input Reference (MIR), including input date, with Sender's address (Output message)
        /// </summary>
        public string MessageInputReference { get; set; }

        /// <summary>
        /// Output date with respect to Receiver (Output message)
        /// </summary>
        public string OutputDate { get; set; }

        /// <summary>
        /// Output time with respect to Receiver (Output message)
        /// </summary>
        public string OutputTime { get; set; }

        public override string BlockPattern
        {
            get { return @"{2:(?:[\w]{47}|[\w]{21})}"; }
        }
    }
}
