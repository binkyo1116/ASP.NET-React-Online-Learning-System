using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMS.Common.SharedFunctions
{
    public class SharedFunctions
    {
        private static string GetConstantIndentifier(string varName, string studentId)
        {
            switch (varName)
            {
                case "studentid":
                    return studentId;
                default:
                    return null;
            }
        }
        /// <summary>
        /// Replaces all variables in a string with their values defined in a dictionary<br/>
        /// A 'variable' is every substring inside brackets and preceded by '$'.<br/>
        /// Like this: ${variable_name}<br/>
        /// Variable names can't contain '$', '{' or '}'.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="varValuePairs"></param>
        /// <returns name="result">Copy of str, with it's variables initialized</returns>
        public static string InitializeVariablesInString(string str, HashSet<(string varName, string varValue, string ocurrenceNumber)> instancesData, string studentId)
        {
            string result = str;

            var split = Regex.Matches(str, @"\$\{((?:\{(?:\{.*?\}|.)*?\}|.)*?)\}");
            for (int i = 0; i < split.Count; ++i)
            {
                string substr = split[i].ToString();
                if (Regex.IsMatch(substr, @"(\${[^$,]*, *\d\d* *})"))
                {
                    Regex lineSplitter = new Regex(@"\${|,|}");
                    String[] subsplit = lineSplitter.Split(substr).Where(s => s != String.Empty).ToArray();
                    //Asumes the var name doesn't include comma, brackets or '$'
                    string varName = Regex.Replace(subsplit[0], @"(\$)|({)|(})|( )", "");
                    string ocurrenceNumber = Regex.Replace(subsplit[1], @"(\$)|({)|(})|( )", "");
                    if (instancesData.Where(x => x.varName == varName && x.ocurrenceNumber == ocurrenceNumber).Any())
                    {
                        result = result.Replace(substr, instancesData.First(x => x.varName == varName && x.ocurrenceNumber == ocurrenceNumber).varValue);
                    }
                }
                else
                {
                    //Asumes the var name doesn't include brackets or '$'
                    string varName = Regex.Replace(substr, @"(\$)|({)|(})|( )", "");
                    string constant = GetConstantIndentifier(varName, studentId);
                    if (constant != null)
                    {
                        result = result.Replace(substr, constant);
                    }
                    else if (instancesData.Where(x => x.varName == varName && x.ocurrenceNumber == null).Any())
                    {
                        result = result.Replace(substr, instancesData.First(x => x.varName == varName && x.ocurrenceNumber == null).varValue);
                    }
                }
            }
            return result;
        }

    }
}
