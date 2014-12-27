using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    public class BodyBlock : FinMessageBlock
    {
        private const string bodyHeaderRegex = "{4:\n(?:[:\w\\\/])+\n-}";
        private const string bodyRegex = @"(?<body>:[A-Z\d]{2,3}:)";

        public override string BlockID
        {
            get { return "4"; }
        }

        public override void BuildBlock(string record)
        {
            throw new NotImplementedException();

            //Regex bodyRegex = new Regex(inputAppBlockRegex);
            //var match = bodyRegex.Match(record);
            //if (!match.Success)
            //    throw new FormatException("invalid basic block!");
        }
    }
}
