using Newtonsoft.Json;
using NKAPIService.API.System.Model;
using PredefineConstant.Enum.Analysis.EventType;
using System;

namespace NKAPIService.API.Converter
{
    public class LabelListConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Label);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<int>(reader);
                    return new Label { LabelId = integerValue };
                case JsonToken.String:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new Label { LabelName = stringValue };
            }
            throw new Exception("Cannot unmarshal type Label");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Label)untypedValue;
            if (value.LabelId != null)
            {
                serializer.Serialize(writer, value.LabelId);
                return;
            }
            if (value.LabelName != null)
            {
                serializer.Serialize(writer, value.LabelName);
                return;
            }
            throw new Exception("Cannot marshal type Label");
        }

    }
}
