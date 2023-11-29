using System;

namespace PredefineConstant.Enum.Analysis
{
    [Flags]
    public enum ObjectType
    {
        Person = 0,
        Vehicle = 1 << 1,
        Face = 1 << 2,
        Fire = 1 << 3,
        Head = 1 << 4,
        Etc = 1 << 5,
    }
}
