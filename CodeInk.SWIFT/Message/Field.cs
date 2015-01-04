using System;
using System.Collections.Generic;

namespace CodeInk.SWIFT.Message
{
    /// <summary>
    /// fin message field
    /// </summary>
    public abstract class Field : IField
    {
        public Field()
            : this(null) { }

        public Field(string[] data)
        {
            Data = data;
        }

        /// <summary>
        /// Get field data
        /// </summary>
        /// <param name="lineNumber">specified line number</param>
        /// <returns>return field data</returns>
        protected string GetDataLine(int lineNumber = 1)
        {
            if (lineNumber < 1)
                throw new ArgumentException("lineNumber can not be less than 1.");

            if (Data == null || Data.Length < lineNumber)
                return null;

            return Data[lineNumber - 1];
        }

        protected internal virtual string[] Data { get; set; }

        /// <summary>
        /// Field identifier
        /// </summary>
        public abstract string Id { get; }
    }

    public interface IField
    {
        string Id { get; }
    }
}
