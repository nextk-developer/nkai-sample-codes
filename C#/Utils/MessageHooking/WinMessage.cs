namespace Utils.MessageHooking
{
    public enum WindowMessage
    {
        None                = -1,
        VK_ESCAPE           = 0x1B,
        WM_KEYDOWN          = 0x0100,
        WM_KEYUP            = 0x0101,
        WM_MOUSEMOVE        = 0x200,
        WM_LBUTTONDOWN      = 0x0201,
        WM_LBUTTONDBLCLK    = 0x0203,
        WM_RBUTTONDOWN      = 0x0204,
    }
}
