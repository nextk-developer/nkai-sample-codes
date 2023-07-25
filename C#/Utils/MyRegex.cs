using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    public class MyRegex
    {
        public  static (string uri, string path, Dictionary<string, DateTime> param) SplitUrinParam(string uri, [CallerMemberName] string caller= "")
        {
            Dictionary<string, DateTime> dic = new Dictionary<string, DateTime>();

            string regexPattern = @"^(?<s1>(?<s0>[^:/\?#]+):)?(?<a1>"
               + @"//(?<a0>[^/\?#]*))?(?<p0>[^\?#]*)"
               + @"(?<q1>\?(?<q0>[^#]*))?"
               + @"(?<f1>#(?<f0>.*))?";

            Regex re = new Regex(regexPattern, RegexOptions.ExplicitCapture);
            Match m = re.Match(uri);

            var query = m.Groups["q1"].Value;
            var param = query.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var p in param)
            {
                var tmp = p.Split('=');

                if (tmp.Length == 2)
                {
                    long i;
                    if (Int64.TryParse(tmp[1], out i))
                    {
                        DateTime dt = new DateTime(i);

                        dic.Add(tmp[0], dt);
                    }
                }
            }

            string exclusionParam = uri;
            if (!string.IsNullOrEmpty(query))
                exclusionParam = uri.Replace(query, "");

            var result = (exclusionParam, m.Groups["p0"].Value, dic);

            return result;
        }
    }
}
