using Newtonsoft.Json;
using NKMeta.Converter;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PredefineConstant
{
    public class EventInfo
    {
        public double AbnormalScore { get; set; }
        public bool IsDetected { get; set; }
        public bool IsTracked { get; set; }
        public bool IsEvent { get; set; }
        public ClassId ClassID { get; set; }
        public double ObjectProb { get; set; }
        public int ObjectID { get; set; } = -1;
        public int EventID { get; set; } = -1;
        public Progress EventStatus { get; set; }
        public IntegrationEventType EventType { get; set; }
        public RoiInfo RoiInfo;

        /// <summary>
        /// 초 단위
        /// </summary>
        public double StayTime { get; set; }
        public byte[] ImageBuffer { get; set; }
        public RectangleF ImageRect { get; set; }
        public RectangleF InnerImageRect { get; set; }
        public ObjectColor ObjectColor { get; set; }
        public EventDetail EventDetail { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class ObjectColor
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    public class RoiObjectCounting
    {
        public int ObjectCount { get; set; }
        public int EnteredCount { get; set; }
        public int ExitedCount { get; set; }
    }

    public class RoiAggregatedData
    {
        public double Max { get; set; }
        public double Min { get; set; }
        public double Avg { get; set; }
        public double Cur { get; set; }
    }

    public class RoiInfo
    {
        public string RoiUid { get; set; }
        public string RoiName { get; set; }
        public RoiNumber RoiNumber { get; set; }
        public ObjectColor RoiColor { get; set; }

        public double AvgStayTime { get; set; }
        public RoiObjectCounting RoiObjectCounting { get; set; }

        public Dictionary<IntegrationEventType, RoiAggregatedData> RoiAggregatedDataItems { get; set; }
    }

    public class ObjectMeta : IEquatable<ObjectMeta>, IComparable<ObjectMeta>
    {
        public string NodeUID { get; }
        public string CameraUID { get; set; }
        public string CameraName { get; set; }
        public List<EventInfo> EventList { get; set; }
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public DateTime TimeStamp { get; set; }
        public int FrameNumber { get; set; }
        public byte[] FrameImage { get; set; }

        public int CompareTo(ObjectMeta other)
        {
            // A null value means that this object is greater.
            if (other == null)
                return 1;
            else
                return this.TimeStamp.CompareTo(other.TimeStamp);
        }

        public bool Equals(ObjectMeta other)
        {
            if (other == null) return false;
            return (this.TimeStamp.Equals(other.TimeStamp));
        }
    }



    public class Vehicle
    {
        public string[] Alphabet { get; } =
        {
            "가", "나", "다", "라", "마", "바", "사", "아", "자", "하",
            "거", "너", "더", "러", "머", "버", "서", "어", "저", "허",
            "고", "노", "도", "로", "모", "보", "소", "오", "조", "호",
            "구", "누", "두", "루", "무", "부", "수", "우", "주", "배",
            "울", "산", "대", "인", "천", "광", "전", "경", "기", "충",
            "전", "강", "원", "북", "남", "제",
            "공", "해", "육", "합", "국",
            "외", "교", "준", "영", "협", "정", "표",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "-"
        };

        public byte[] LicenseImageBuffer { get; set; }
        public RectangleF LicenseImageRect { get; set; }
        public List<int> LicensePlate { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new(125);

            foreach (var plate in LicensePlate)
            {
                sb.Append(Alphabet[plate]);
            }

            return sb.ToString();
        }
    }

    public class EventDetail
    {
        public double EventScore { get; set; }
        public string FaceUID { get; set; }
        public List<string> SimliarList { get; set; }
        public Face Face { get; set; }
        public GPS GPS { get; set; }

        [JsonConverter(typeof(PoseKeyPointsToStringConverter))]
        public Dictionary<PoseKeyPoints, KeyPoint> KeyPoints { get; set; }
        public Vehicle Vehicle { get; set; }
    }


    public class KeyPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Score { get; set; }
    }

    public class Face
    {
        public string Name { get; set; }
        public float Age { get; set; }
        public Gender Gender { get; set; }
        public Identifier Identify { get; set; }
    }

    public class GPS
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
