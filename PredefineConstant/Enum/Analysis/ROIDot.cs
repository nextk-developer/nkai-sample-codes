using Newtonsoft.Json;
using System;
using System.Drawing;

namespace PredefineConstant.Enum.Analysis
{
    public class ROIDot
    {
        [JsonProperty("x")]
        public double X { get; set; }
        [JsonProperty("y")]
        public double Y { get; set; }

        public static bool operator ==(ROIDot v1, ROIDot v2)
        {
            return v1.X == v2.X &&
                v1.Y == v2.Y;
        }
        public static bool operator !=(ROIDot v1, ROIDot v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object obj)
        {
            return this == (ROIDot)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class NKSize : IEquatable<NKSize>
    {
        [JsonProperty("width")]
        public double Width { get; set; }
        [JsonProperty("height")]
        public double Height { get; set; }

        public NKSize() { }
        public NKSize(SizeF size) : this()
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public NKSize(double w, double h) : this()
        {
            this.Width = w;
            this.Height = h;
        }

        public bool Equals(NKSize other)
        {
            return this.Width == other.Width &&
                    this.Height == other.Height;
        }
    }
}
