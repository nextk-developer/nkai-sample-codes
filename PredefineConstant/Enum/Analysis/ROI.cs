using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PredefineConstant.Converter;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;

namespace PredefineConstant.Enum.Analysis
{
    public class MinMaxSize : IEquatable<MinMaxSize>
    {
        [JsonProperty("min_detection_size")]
        public NKSize MinDetectSize { get; set; }
        [JsonProperty("max_detection_size")]
        public NKSize MaxDetectSize { get; set; }

        public bool Equals(MinMaxSize target)
        {
            if (target == null) return false;

            return MinDetectSize == target.MinDetectSize &&
                MaxDetectSize == target.MaxDetectSize;
        }

        internal MinMaxSize Clone()
        {
            return new MinMaxSize()
            {
                MinDetectSize = MinDetectSize,
                MaxDetectSize = MaxDetectSize
            };
        }
    }

    public class RoiPoint : IEquatable<RoiPoint>
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("roi_type")]
        public DrawingType RoiType { get; set; }
        [JsonProperty("number")]
        public RoiNumber RoiNumber { get; set; }
        [JsonProperty("points")]
        public List<ROIDot> Points { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }

        public bool Equals(RoiPoint other)
        {
            if (other == null) return false;

            return RoiType == other.RoiType &&
                RoiNumber == other.RoiNumber &&
                Points == other.Points &&
                Description == other.Description;
        }
    }


    public class RoiModel : IEquatable<RoiModel>
    {
        [JsonProperty("roi_id")]
        public string RoiId { get; set; }
        [JsonProperty("roi_name")]

        public string RoiName { get; set; }
        [JsonProperty("roi_type")]
        public DrawingType RoiType { get; set; }
        [JsonProperty("evt_type")]
        [JsonConverter(typeof(EventToStringConverter))]
        public IntegrationEventType EventType { get; set; }
        [JsonProperty("object_group")]
        public ObjectType ObjectType { get; set; }
        [JsonProperty("object_filter")]
        public List<ClassId> ObjectsFilter { get; set; }
        [JsonProperty("roi_points")]
        public List<RoiPoint> RoiPoints { get; set; }
        [JsonProperty("min_max_size")]
        public MinMaxSize MinMaxSize { get; set; }
        [JsonProperty("params")]
        public Dictionary<string, string>? Params { get; set; }

        public RoiModel Clone()
        {
            return new RoiModel()
            {
                Params = this.Params,
                EventType = this.EventType,
                RoiId = this.RoiId,
                RoiName = this.RoiName,
                RoiType = this.RoiType,
                ObjectsFilter = this.ObjectsFilter,
                MinMaxSize = this.MinMaxSize.Clone(),
                ObjectType = this.ObjectType,
                RoiPoints = this.RoiPoints,
            };
        }

        public bool Equals(RoiModel target)
        {
            if (target == null) return false;
            return this.RoiId == target.RoiId &&
                this.RoiName == target.RoiName &&
                this.EventType == target.EventType &&
                this.ObjectsFilter == target.ObjectsFilter &&
                this.RoiPoints == target.RoiPoints &&
                this.MinMaxSize.MaxDetectSize.Width == target.MinMaxSize.MaxDetectSize.Width &&
                this.MinMaxSize.MaxDetectSize.Height == target.MinMaxSize.MaxDetectSize.Height &&
                this.MinMaxSize.MinDetectSize.Width == target.MinMaxSize.MinDetectSize.Width &&
                this.MinMaxSize.MinDetectSize.Height == target.MinMaxSize.MinDetectSize.Height &&
                Compare(this.Params, target.Params);
        }

        private bool Compare(Dictionary<string, string> params1, Dictionary<string, string> params2)
        {
            if (params1 == null && params2 == null) return true;
            if (params1 == null || params2 == null) return false;
            if (params1.Keys.Count != params2.Keys.Count) return false;

            foreach (var key in params1.Keys)
            {
                var value1 = params1[key];
                if (!params2.ContainsKey(key)) return false;

                var value2 = params2[key];

                if (value1 != value2) return false;
            }

            return true;

        }
    }
}
