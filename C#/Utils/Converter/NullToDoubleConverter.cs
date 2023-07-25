using Newtonsoft.Json;
using System;

namespace Utils.Converter
{
    public class NullToDoubleConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            double val = 0;
            try
            {
                var tmp = serializer.Deserialize(reader);
                if (tmp != null)
                {
                    if (tmp != null && double.TryParse(tmp.ToString(), out double result))
                    {
                        val = result;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return val;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}