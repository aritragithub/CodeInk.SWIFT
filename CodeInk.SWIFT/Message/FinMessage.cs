using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    /// <summary>
    /// fin message
    /// </summary>
    public class FinMessage
    {
        public BasicBlock BasicBlock { get; set; }
        public BasicBlock AckBasicBlock { get; set; }
        public ApplicationBlock ApplicationBlock { get; set; }
        public UserBlock UserBlock { get; set; }
        public BodyBlock BodyBlock { get; set; }
        public TrailerBlock TrailerBlock { get; set; }

        /// <summary>
        /// Read fin message
        /// </summary>
        /// <param name="message">fin message</param>
        /// <returns>return object of <code>CodeInk.SWIFT.Message.FinMessage</code></returns>
        public static FinMessage ReadMessage(string message)
        {
            FinMessage finMessage = new FinMessage();

            List<FinMessageBlock> structure = new List<FinMessageBlock>();

            structure.Add(new BasicBlock());
            structure.Add(new ApplicationBlock());
            structure.Add(new UserBlock());
            structure.Add(new BodyBlock());
            structure.Add(new TrailerBlock());

            foreach (var item in structure)
            {
                Regex reg = new Regex(item.BlockPattern);
                var match = reg.Match(message);
                if (match.Success)
                {
                    message = reg.Replace(message, string.Empty, 1);
                    item.BuildBlock(match.Value);
                }
            }

            finMessage.BasicBlock = structure.Single(b => b is BasicBlock) as BasicBlock;
            finMessage.ApplicationBlock = structure.Single(b => b is ApplicationBlock) as ApplicationBlock;
            finMessage.UserBlock = structure.Single(b => b is UserBlock) as UserBlock;
            finMessage.BodyBlock = structure.Single(b => b is BodyBlock) as BodyBlock;
            finMessage.TrailerBlock = structure.Single(b => b is TrailerBlock) as TrailerBlock;

            return finMessage;
        }
    }
}
