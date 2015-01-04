using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeInk.SWIFT.Message
{
    /// <summary>
    /// fin message body block
    /// </summary>
    public class BodyBlock : FinMessageBlock
    {
        private Dictionary<string, List<GeneralField>> Fields;

        private const string bodyHeaderRegex = @"{4:\r\n(?:[\sa-zA-Z0-9,\.:'\+\(\)\?'\-\/])+\r\n-}";
        private const string fieldRegex = @"^(?:[:](?<field>[\d]{2}[A-Z]{0,1}):)";

        public override string BlockID
        {
            get { return "4"; }
        }

        public override void BuildBlock(string record)
        {
            var bodyRegex = new Regex(bodyHeaderRegex, RegexOptions.Compiled);
            var match = bodyRegex.Match(record);
            if (!match.Success)
                throw new FormatException("invalid Body block!");

            Fields = new Dictionary<string, List<GeneralField>>();

            var lines = record.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var lineCount = lines.Count();

            var fieldline = new Regex(fieldRegex);

            for (int linenumber = 0; linenumber < lineCount; linenumber++)
            {
                var fieldmatch = fieldline.Match(lines[linenumber]);
                if (fieldmatch.Success && fieldmatch.Groups["field"].Success)
                {
                    var fieldId = fieldmatch.Groups["field"].Value;
                    var fieldData = new List<string>();
                    fieldData.Add(new string(lines[linenumber].Skip(fieldId.Length + 2).ToArray()));

                    linenumber++; // check next line for data
                    // take data till we find next field line or end of body block
                    while (lines[linenumber] != "-}" && !fieldline.Match(lines[linenumber]).Success)
                    {
                        fieldData.Add(lines[linenumber]);
                        linenumber++;
                    }
                    linenumber--; // revert back if field record is found

                    var field = new GeneralField { Data = fieldData };
                    if (!Fields.ContainsKey(fieldId))
                        Fields[fieldId] = new List<GeneralField>(); ;

                    Fields[fieldId].Add(field);
                }
            }
        }

        /// <summary>
        /// Get values for corresponding field
        /// </summary>
        /// <param name="field">field identifier</param>
        /// <param name="lineNumber">get field data form line number</param>
        /// <returns>return fields data</returns>
        public string[] GetFieldValue(string field, int lineNumber = 1)
        {
            if (string.IsNullOrWhiteSpace(field))
                throw new ArgumentException("invalid field requested.");

            if (lineNumber < 1)
                throw new ArgumentException("lineNumber can not be less than 1.");

            if (Fields == null || !Fields.ContainsKey(field))
                return null;
            var data = Fields[field].ElementAtOrDefault(lineNumber - 1);
            if (data == null)
                return null;

            return data.Data.ToArray(); ;
        }

        /// <summary>
        /// Get strogly typed field data 
        /// </summary>
        /// <typeparam name="T">Field type</typeparam>
        /// <param name="lineNumber">get field data form line number</param>
        /// <returns>return object of T</returns>
        public T GetField<T>(int lineNumber = 1) where T : Field, new()
        {
            T typeObj = new T();
            typeObj = (T)Activator.CreateInstance(typeof(T), new object[] { GetFieldValue(typeObj.Id, lineNumber) });
            return typeObj;
        }

        public override string BlockPattern
        {
            get { return bodyHeaderRegex; }
        }
    }

    class GeneralField
    {
        public List<string> Data { get; set; }
    }
}


