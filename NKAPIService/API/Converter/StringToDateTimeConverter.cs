using Newtonsoft.Json;
using System;

namespace NKAPIService.API.Converter
{
    public class StringToDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime datetime = DateTime.MinValue;
            try
            {
                var tmp = serializer.Deserialize<string>(reader);

                if (DateTime.TryParse(tmp, out DateTime result))
                {
                    datetime = result;
                }
            }
            catch (Exception exc)
            {

                Console.WriteLine(exc.Message);
            }

            return datetime;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string val = "";
            try
            {
                if (value is DateTime result)
                {
                    val = result.ToString();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            writer.WriteValue(val);
        }
    }
}
