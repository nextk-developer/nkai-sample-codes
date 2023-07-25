using Newtonsoft.Json;
using PredefineConstant;
using PredefineConstant.Enum.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NKMeta.Converter
{
    public class PoseKeyPointsToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Dictionary<PoseKeyPoints, KeyPoint> dic = new();
            try
            {
                var tmp = serializer.Deserialize<Dictionary<string, KeyPoint>>(reader);

                dic = tmp.ToDictionary(k => (PoseKeyPoints)Convert.ToInt32(k.Key), x => x.Value);
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
