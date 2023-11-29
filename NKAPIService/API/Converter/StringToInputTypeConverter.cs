using Newtonsoft.Json;
using NKAPIService.API.Channel.Models;
using System;

namespace NKAPIService.API.Converter
{
    class StringToInputTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            InputType inputType = InputType.SRC_IPCAM_NORMAL;
            try
            {
                var tmp = serializer.Deserialize<string>(reader);

                if (Enum.TryParse(tmp, out InputType result))
                {
                    inputType = result;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }

            return inputType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            int val = 0;
            try
            {
                if (Enum.TryParse(value.ToString(), out InputType result))
                {
                    val = (int)result;

                }
            }
            catch (Exception e)
            {

            }

            writer.WriteValue(val);
        }
    }
}
