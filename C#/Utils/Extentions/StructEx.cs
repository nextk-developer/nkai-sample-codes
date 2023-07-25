using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Utils.Extentions
{
    public static class StructEx
    {
        public static bool IsDefault<T>(this T pair) where T : struct
            => pair.Equals(default(T));
    }
}
