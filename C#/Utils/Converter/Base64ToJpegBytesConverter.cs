using Newtonsoft.Json;
using System;
using System.Drawing;
using Utils.Extentions;

namespace Utils.Converter
{
    public class Base64ToJpegBytesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            byte[] bmp = null;
            try
            {
                var tmp = serializer.Deserialize(reader);
                if (tmp != null)
                {
                    var jpegBytes = Convert.FromBase64String(tmp.ToString());

                    if (jpegBytes != null)
                    {
                        bmp = jpegBytes;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return bmp;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is byte[] jpegBytes)
            {
                try
                {
                    string base64 = Convert.ToBase64String(jpegBytes);
                    writer.WriteValue(base64);
                }
                catch (Exception e)
                {
                    writer.WriteValue("");
                }
            }
        }

    }
}