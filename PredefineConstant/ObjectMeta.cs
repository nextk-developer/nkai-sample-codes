using Newtonsoft.Json;
using NKMeta.Converter;
using PredefineConstant.Converter;
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
        [JsonProperty("abnormal_score")]
        public double AbnormalScore { get; set; }
        [JsonProperty("is_detected")]
        public bool IsDetected { get; set; }
        [JsonProperty("is_tracked")]
        public bool IsTracked { get; set; }
        [JsonProperty("is_event")]
        public bool IsEvent { get; set; }
        [JsonProperty("class_id")]
        public ClassId ClassID { get; set; }
        [JsonProperty("object_prob")] 
        public double ObjectProb { get; set; }
        [JsonProperty("object_id")]
        public int ObjectID { get; set; } = -1;
        [JsonProperty("event_id")]
        public int EventID { get; set; } = -1;
        [JsonProperty("event_status")]
        public Progress EventStatus { get; set; }
        [JsonProperty("event_type")]
        public IntegrationEventType EventType { get; set; }
        [JsonProperty("roi_info")]
        public RoiInfo RoiInfo { get; set; }

        /// <summary>
        /// 초 단위
        /// </summary>
        [JsonProperty("stay_time")]
        public double StayTime { get; set; }
        [JsonProperty("image_buffer")]
        public byte[] ImageBuffer { get; set; }
        [JsonProperty("image_rect")]
        public RectangleF ImageRect { get; set; }
        [JsonProperty("inner_image_rect")]
        public RectangleF InnerImageRect { get; set; }
        [JsonProperty("object_color")]
        public ObjectColor ObjectColor { get; set; }
        [JsonProperty("event_detail")]
        public EventDetail EventDetail { get; set; }
        [JsonProperty("time_stamp")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("cls_items")]
        public List<string> ClsItems { get; set; }
    }

    public class ObjectColor
    {
        [JsonProperty("r")]
        public int R { get; set; }
        [JsonProperty("g")]
        public int G { get; set; }
        [JsonProperty("b")]
        public int B { get; set; }
    }

    public class RoiObjectCounting
    {
        [JsonProperty("object_count")]
        public int ObjectCount { get; set; }
        [JsonProperty("entered_count")]
        public int EnteredCount { get; set; }
        [JsonProperty("exited_count")]
        public int ExitedCount { get; set; }
    }

    public class RoiAggregatedData
    {
        [JsonProperty("max")]
        public double? Max { get; set; }
        [JsonProperty("min")]
        public double? Min { get; set; }
        [JsonProperty("avg")]
        public double? Avg { get; set; }
        [JsonProperty("cur")]
        public double? Cur { get; set; }
    }

    public class RoiInfo
    {
        [JsonProperty("roi")]
        public RoiModel Roi { get; set; }
        [JsonProperty("avg_stay_time")]
        public double AvgStayTime { get; set; }
        [JsonProperty("roi_object_counting")]
        public RoiObjectCounting RoiObjectCounting { get; set; }

        [JsonProperty("roi_aggregated_data_items")]
        [JsonConverter(typeof(RoiAggregatedDataConverter))]
        public Dictionary<IntegrationEventType, RoiAggregatedData> RoiAggregatedDataItems { get; set; }
    }

    public class ObjectMeta : IEquatable<ObjectMeta>, IComparable<ObjectMeta>
    {
        [JsonProperty("camera_uid")]
        public string CameraUID { get; set; }
        [JsonProperty("camera_name")]
        public string CameraName { get; set; }
        [JsonProperty("event_list")]
        public List<EventInfo> EventList { get; set; }
        [JsonProperty("frame_width")]
        public int FrameWidth { get; set; }
        [JsonProperty("frame_height")]
        public int FrameHeight { get; set; }
        [JsonProperty("time_stamp")]
        public DateTime TimeStamp { get; set; }
        [JsonProperty("frame_number")]
        public int FrameNumber { get; set; }
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

        [JsonProperty("license_image_buffer")]
        public byte[] LicenseImageBuffer { get; set; }
        [JsonProperty("license_image_rect")]
        public RectangleF LicenseImageRect { get; set; }
        [JsonProperty("license_plate")]
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
        [JsonProperty("event_score")]
        public double EventScore { get; set; }
        [JsonProperty("face_id")]
        public string FaceUID { get; set; }
        [JsonProperty("simliar_list")]
        public List<string> SimliarList { get; set; }
        [JsonProperty("face")]
        public Face Face { get; set; }
        [JsonProperty("gps")]
        public GPS GPS { get; set; }

        [JsonProperty("key_points")]
        [JsonConverter(typeof(PoseKeyPointsToStringConverter))]
        public Dictionary<PoseKeyPoints, KeyPoint> KeyPoints { get; set; }
        [JsonProperty("vehicle")]
        public Vehicle Vehicle { get; set; }
    }


    public class KeyPoint
    {
        [JsonProperty("x")]
        public float X { get; set; }
        [JsonProperty("y")]
        public float Y { get; set; }
        [JsonProperty("score")]
        public float Score { get; set; }
    }

    public class Face
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("age")]
        public float Age { get; set; }
        [JsonProperty("gender")]
        public Gender Gender { get; set; }
        [JsonProperty("identify")]
        public Identifier Identify { get; set; }
        [JsonProperty("memo")]
        public string Memo { get; set; }
    }

    public class GPS
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
