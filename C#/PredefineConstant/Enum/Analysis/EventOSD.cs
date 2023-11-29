using System;

namespace PredefineConstant.Enum.Analysis
{
    [Flags]
    public enum EventOSD
    {
        None = 0,
        DetectBox = 1 << 1,
        EventBox = 1 << 2,
        EventDetail = 1 << 3 + EventBox,
        EventDetailEx = 1 << 4 + EventDetail,
        All = DetectBox + EventDetailEx
    }
}
