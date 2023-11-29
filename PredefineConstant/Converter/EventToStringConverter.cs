using Newtonsoft.Json;
using PredefineConstant.Enum.Analysis.EventType;
using System;

namespace PredefineConstant.Converter
{
    public class EventToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IntegrationEventType eventType = IntegrationEventType.AllDetect;
            try
            {
                var tmp = serializer.Deserialize<string>(reader);

                if (System.Enum.TryParse(tmp.ToString(), out IntegrationEventType result))
                {
                    eventType = result;
                }
            }
            catch (Exception exc)
            {

                Console.WriteLine(exc.Message);
            }

            return eventType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
