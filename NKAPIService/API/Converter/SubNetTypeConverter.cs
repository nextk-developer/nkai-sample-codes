using Newtonsoft.Json;
using NKAPIService.API.System.Model;
using System;

namespace NKAPIService.API.Converter
{
    internal class SubNetTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DetectorSubType) || t == typeof(DetectorSubType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "m":
                    return DetectorSubType.M;
                case "n":
                    return DetectorSubType.N;
                case "s":
                    return DetectorSubType.S;
                case "x":
                    return DetectorSubType.X;
                case "l":
                    return DetectorSubType.L;
            }
            return DetectorSubType.None;
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, "");
                return;
            }
            var value = (DetectorSubType)untypedValue;
            switch (value)
            {
                case DetectorSubType.M:
                    serializer.Serialize(writer, "m");
                    return;
                case DetectorSubType.N:
                    serializer.Serialize(writer, "n");
                    return;
                case DetectorSubType.S:
                    serializer.Serialize(writer, "s");
                    return;
                case DetectorSubType.X:
                    serializer.Serialize(writer, "x");
                    return;
                case DetectorSubType.L:
                    serializer.Serialize(writer, "l");
                    return;
            }
            serializer.Serialize(writer, "");
        }
    }
}