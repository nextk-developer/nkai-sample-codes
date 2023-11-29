using Newtonsoft.Json;
using System;

namespace NKAPIService.API.Converter
{
    public class IntToErrorCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ErrorCode code = ErrorCode.SUCCESS;
            try
            {
                var tmp = serializer.Deserialize(reader);

                if (int.TryParse(tmp.ToString(), out int result))
                {
                    code = (ErrorCode)result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return code;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int val = 0;
            try
            {
                if (Enum.TryParse(value.ToString(), out ErrorCode result))
                {
                    val = (int)result;

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
