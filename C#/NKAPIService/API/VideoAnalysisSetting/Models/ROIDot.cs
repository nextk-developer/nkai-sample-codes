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
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }

        public RoiSize() { }
        public RoiSize(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public RoiSize(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        public Size ToSize() { return new Size(this.Width, this.Height); }
    }
}
