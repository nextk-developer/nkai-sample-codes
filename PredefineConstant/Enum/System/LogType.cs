using System;

namespace PredefineConstant.Enum.System
{
    [Flags]
    public enum LogType
    {
        None        = 0,
        Info        = 0x01 << 0,
        Debug       = 0x01 << 1,
        Trace       = 0x01 << 2,
        Critical    = 0x01 << 3,
        Error       = 0x01 << 4,
        Warning     = 0x01 << 5,
        Event       = 0x01 << 6,
    }
}
