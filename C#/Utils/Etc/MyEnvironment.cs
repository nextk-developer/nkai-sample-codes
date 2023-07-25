using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.Etc
{
    public static class MyEnvironment
    {
        public static void AddEnvironmentPaths(string[] paths)
        {
            string path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            path += ";" + string.Join(";", paths);

            Environment.SetEnvironmentVariable("PATH", path);
        }
    }
}
