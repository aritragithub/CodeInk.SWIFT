﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeInk.SWIFT.Message
{
    public class UserBlock : FinMessageBlock
    {
        private const string userBlockRegex = @"^{3:(?<data>(?:{[\d]{1,3}:[\w]+})+)}$";
        public Dictionary<string, string> Tags;

        public UserBlock()
        {
            Tags = new Dictionary<string, string>();
        }
        public override string BlockID
        {
            get { return "3"; }
        }

        public override void BuildBlock(string record)
        {
            var userRegex = new Regex(userBlockRegex);
            var match = userRegex.Match(record);


            if (!match.Success)
                throw new FormatException("invalid User block!");

            var data = match.Groups["data"].Value;
            var splitData = data.Split(new string[] { "{", "}", ":" }, StringSplitOptions.RemoveEmptyEntries);

            Tags = new Dictionary<string, string>();

            for (int i = 0; i < splitData.Length; i += 2)
            {
                Tags.Add(splitData[i], splitData[i + 1]);
            }
        }

        /// <summary>
        /// Get user block tag value 
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
