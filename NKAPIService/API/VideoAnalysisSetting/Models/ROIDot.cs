using Newtonsoft.Json;
using System.Drawing;

namespace NKAPIService.API.VideoAnalysisSetting.Models
{
    public class ROIDot
    {
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public class RoiSize
    {
        [JsonProperty("width")]
        public double Width { get; set; }
        [JsonProperty("height")]
        public double Height { get; set; }

        public RoiSize() { }
        public RoiSize(SizeF size) : this()
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public RoiSize(double w, double h) : this()
        {
            this.Width = w;
            this.Height = h;
        }
    }
}
