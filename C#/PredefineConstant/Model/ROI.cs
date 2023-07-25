using PredefineConstant.Enum.Analysis;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PredefineConstant.Model
{
    public class ROI
    {
        public DrawingType DrawingType { get; set; }
        public List<PointF> Points { get; set; }
        public List<PointF> PointsSub { get; set; }

        public ROI Clone()
        {
            return new ROI()
            {
                DrawingType = this.DrawingType,
                Points = this.Points.ToList(),
                PointsSub = this.PointsSub.ToList(),
            };
        }
    }
}
