using PredefineConstant.Enum.Analysis;

namespace PredefineConstant.Extenstion
{
    public static class ObjectsColorConverter
    {
        private static readonly System.Drawing.Color _colorDefualt = System.Drawing.Color.White;
        private static readonly System.Drawing.Color _colorEvent = System.Drawing.Color.FromArgb(255, 255, 20, 84);
        private static readonly System.Drawing.Color _colorFillEvent = System.Drawing.Color.FromArgb(50, 255, 20, 84);
        private static readonly System.Drawing.Color _colorFillDefualt = System.Drawing.Color.FromArgb(30, 20, 215, 255);
        private static readonly System.Drawing.Color _colorMatch = System.Drawing.Color.FromArgb(255, 25, 255, 25);
        private static readonly System.Drawing.Color _colorNormalFont = System.Drawing.Color.LightGreen;
        private static readonly System.Drawing.Color _colorMatchFont = System.Drawing.Color.White;

        //roi
        private static readonly System.Drawing.Color _colorNormalROI = System.Drawing.Color.SkyBlue;

        //face
        public static (int rectColor, System.Drawing.Color fontColor) ToColorByFaceScore(this Identifier identifier, float score)
        {
            if (score > 0)
            {
                return (ToScalar(_colorMatch), _colorMatchFont);
            }
            else
            {
                return (ToScalar(_colorEvent), _colorNormalFont);
            }
        }

        public static int ToScalarColor(this Identifier identifier)
        {
            if (identifier == Identifier.Black)
                return ToScalar(System.Drawing.Color.FromArgb(50, 0, 0, 0));
            else if (identifier == Identifier.White)
                return ToScalar(System.Drawing.Color.FromArgb(50, 255, 255, 255));

            return ToScalar(System.Drawing.Color.FromArgb(0, 0, 0, 0));
        }
        //abnormal
        public static int ToScalarColor(bool bEvent)
        {
            if (bEvent == true)
                return ToScalar(_colorFillEvent);
            else
                return ToScalar(_colorFillDefualt);
        }
        public static int ToBoxColor(bool bEvent)
        {
            if (bEvent)
                return ToScalar(_colorEvent);
            else
                return ToScalar(_colorDefualt);
        }
        public static System.Drawing.Color GetDefaultFontColor()
        {
            return _colorNormalFont;
        }

        public static System.Drawing.Color ToROIColor(bool bEvent)
        {
            if (bEvent)
                return _colorEvent;
            else
                return _colorNormalROI;
        }

        //public
        public static int ToScalar(System.Drawing.Color color)
        {
            var scalar = 0;

            if (color.A != 0)
            {
                var a = color.A + 1;
                scalar = (color.A << 24)
                         | ((byte)((color.R * a) >> 8) << 16)
                         | ((byte)((color.G * a) >> 8) << 8)
                         | ((byte)((color.B * a) >> 8));
            }

            return scalar;
        }
    }
}
