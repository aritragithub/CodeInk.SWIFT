using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    public abstract class FinMessageBlock
    {
        /// <summary>
        /// Get block identifier
        /// </summary>
        public abstract string BlockID { get; }

        /// <summary>
        /// Build message block from single message block string
        /// </summary>
        /// <param name="record">message string</param>
        public abstract void BuildBlock(string record);

        /// <summary>
        /// Regular expression to parse block data from message string
        /// </summary>
        public abstract string BlockPattern { get; }
    }
}
