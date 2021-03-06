﻿using System;
using System.Text.RegularExpressions;

namespace CodeInk.SWIFT.Message
{
    /// <summary>
    /// fin message basic block
    /// </summary>
    public class BasicBlock : FinMessageBlock
    {
        private const string basicBlockRegex = @"{1:(?<appid>[FAL]{1})(?<serviceid>[\d]{2})(?<ltAddress>[A-Za-z]{12})(?<sessionno>[\d]{4})(?<seqno>[\d]{6})}";

        /// <summary>
        /// Build a BasicBlock
        /// </summary>
        /// <param name="record"></param>
        public override void BuildBlock(string record)
        {
            Regex basicRegex = new Regex(basicBlockRegex);
            var match = basicRegex.Match(record);
            if (!match.Success)
                throw new FormatException("invalid Basic block!");

            ApplicationID = match.Groups["appid"].Value;
            ServiceID = match.Groups["serviceid"].Value;
            TerminalAddress = match.Groups["ltAddress"].Value;
            SessionNumber = match.Groups["sessionno"].Value;
            SequenceNumber = match.Groups["seqno"].Value;
        }

        public override string BlockID
        {
            get
            {
                return "1";
            }
        }

        /// <summary>
        /// Application identifier of message 
        /// </summary>
        /// <remarks>
        /// F = FIN (financial application)
        /// A = GPA (general purpose application)
        /// L = GPA (for logins, and so on) 
        /// </remarks>
        public string ApplicationID { get; set; }

        /// <summary>
        /// Service identifier
        /// </summary>
        /// <remarks>
        /// Service ID as follows:
        /// 01 = FIN/GPA
        /// 21 = ACK/NAK 
        /// </remarks>
        public string ServiceID { get; set; }

        /// <summary>
        /// Logical terminal (LT) address 
        /// </summary>
        public string TerminalAddress { get; set; }

        /// <summary>
        /// Session number
        /// </summary>
        public string SessionNumber { get; set; }

        /// <summary>
        /// Sequence number
        /// </summary>
        public string SequenceNumber { get; set; }


        public override string BlockPattern
        {
            get { return @"{1:[\w]{25}}"; }
        }
    }
}
