using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    public class TailerBlock : FinMessageBlock
    {
        private const string userBlockRegex = @"^{5:(?<data>(?:{[\w]{1,3}:[\w]+})+)}$";
        public Dictionary<string, string> Tags;

        public TailerBlock()
        {
            Tags = new Dictionary<string, string>();
        }
        public override string BlockID
        {
            get { return "5"; }
        }

        public override void BuildBlock(string record)
        {
            var userRegex = new Regex(userBlockRegex);
            var match = userRegex.Match(record);


            if (!match.Success)
                throw new FormatException("invalid Tailer block!");

            var data = match.Groups["data"].Value;
            var splitData = data.Split(new string[] { "{", "}", ":" }, StringSplitOptions.RemoveEmptyEntries);

            Tags = new Dictionary<string, string>();

            for (int i = 0; i < splitData.Length; i += 2)
            {
                Tags.Add(splitData[i], splitData[i + 1]);
            }
        }

        /// <summary>
        /// Get tailer block tag value 
        /// </summary>
        /// <param name="tagId">tag identifier</param>
        /// <returns>return data for the tag specifier</returns>
        public string GetTagValue(string tagId)
        {
            string data = null;
            Tags.TryGetValue(tagId, out data);

            return data;
        }
    }
}
