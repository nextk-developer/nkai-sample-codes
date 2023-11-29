using Newtonsoft.Json;
using PredefineConstant;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NKMeta.Converter
{
    public class RoiAggregatedDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<IntegrationEventType, RoiAggregatedData> dic = new();
            try
            {
                var tmp = serializer.Deserialize<Dictionary<string, RoiAggregatedData>>(reader);

                dic = tmp.ToDictionary(k => (IntegrationEventType)Enum.Parse(typeof(IntegrationEventType), k.Key), x => x.Value);
            }
            catch (Exception exc)
            {

                Console.WriteLine(exc.Message);
            }

            return dic;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
