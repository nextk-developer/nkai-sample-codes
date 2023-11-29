using Newtonsoft.Json;
using System.Collections.Generic;

namespace PredefineConstant
{
    public class SensorMeta
    {
        [JsonProperty("Sensors")]
        public List<Sensor> Sensors { get; set; }
    }

    public class Sensor
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Humidity")]
        public int Humidity { get; set; }

        [JsonProperty("Temperature")]
        public double Temperature { get; set; }

        [JsonProperty("AmountOf")]
        public int AmountOf { get; set; }
    }
}
