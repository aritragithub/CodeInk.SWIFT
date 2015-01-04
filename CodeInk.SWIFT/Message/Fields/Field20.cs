using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message.Fields
{
    /// <summary>
    /// Reference field
    /// </summary>
    public class Field20 : Field
    {
        public Field20()
            : base() { }

        public Field20(string[] data) :
            base(data)
        {
            Reference = GetDataLine();
        }

        /// <summary>
        /// Transaction Reference Number 
        /// </summary>
        public string Reference { get; set; }

        public override string Id
        {
            get { return "20"; }
        }
    }
}
