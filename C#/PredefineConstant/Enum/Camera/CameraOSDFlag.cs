using System;

namespace PredefineConstant.Enum.Camera
{
    [Flags]
    public enum CameraOSDFlag
    {
        NONE = 0,
        FPS = 1 << 0,
        RESOLUTION = 1 << 1,
        DETAIL = 1 << 2,
        All = ~(-1 << 3)
    }
}
