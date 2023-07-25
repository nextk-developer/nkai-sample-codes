using System;

namespace PredefineConstant.Enum.Recording
{
    public enum RecordType
    {
        Multiple = -1,
        None = 0,
        Video = 1 << 0,
        Event = 1 << 1,
        VideoNEvent = ~(-1 << 2)
    }
}