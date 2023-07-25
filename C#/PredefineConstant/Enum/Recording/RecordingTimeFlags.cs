using System;

namespace PredefineConstant.Enum.Recording
{
    [Flags]
    public enum RecordTimeFlags
    {
        None = 0,
        _09To18 = 1 << 1,
        _18To24 = 1 << 2,
        _24To09 = 1 << 3,
        EveryTime = ~(-1 << 4)
    }
}
