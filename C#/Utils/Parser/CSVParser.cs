using System.Text.RegularExpressions;

namespace Utils.Parser
{
    public class CSVParser
    {
        public static string[] Parsing(string line)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            return CSVParser.Split(line);
        }
    }
}
