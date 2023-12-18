using Newtonsoft.Json;
using PredefineConstant.Converter;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;

namespace PredefineConstant
{
    public class StatisticsMeta
    {
        public string ChannelUId { get; set; }
        public string ChannelName { get; set; }
        public DateTime DateTime { get; set; }
        public List<StatisticsHour> Hours { get; set; }
    }

    public class StatisticsHour
    {
        public int Hour { get; set; }

        public List<StatisticsMinuteQuarter> MinuteQuarters { get; set; }
    }

    public class StatisticsMinuteQuarter
    {
        public int MinuteQuarter { get; set; }

        public List<StatisticsEventType> EventTypes { get; set; }
    }

    public class StatisticsEventType
    {
        public string RoiUId { get; set; }

        public RoiNumber RoiNumber { get; set; }

        [JsonConverter(typeof(EventToStringConverter))]
        public IntegrationEventType EventType { get; set; }

        public List<StatisticsClassId> ClassIds { get; set; }
    }

    public class StatisticsClassId
    {
        public ClassId ClassId { get; set; }
        public int Count { get; set; }
    }
}
