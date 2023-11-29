using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Camera;
using System;
using System.Collections.Generic;

namespace PredefineConstant.Model.Camera
{
    public class Detail
    {
        public string Host { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Password { get; set; }
        public int GroupIndex { get; set; }
        public string NodeID { get; set; }
        public string CameraUID { get; set; }
        public ChannelType Type { get; set; }
        public DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 0: MainURL, 1: SubURL, 2 => ...any
        /// </summary>
        public List<string> URLs { get; set; }
        public Address Address { get; set; }
        public string MetaServiceHost { get; set; }
    }

    public class Address
    {
        public string Addres { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
