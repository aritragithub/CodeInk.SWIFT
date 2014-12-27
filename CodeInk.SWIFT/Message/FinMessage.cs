using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    public class FinMessage
    {
        public BasicBlock BasicBlock { get; set; }
        public BasicBlock AckBasicBlock { get; set; }
        public ApplicationBlock ApplicationBlock { get; set; }
        public UserBlock UserBlock { get; set; }
        public BodyBlock BodyBlock { get; set; }
        public TailerBlock TailerBlock { get; set; }
    }

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
    }
}
