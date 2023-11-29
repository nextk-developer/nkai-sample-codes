using Newtonsoft.Json;
using NKAPIService.API.Converter;
using System.Collections.Generic;

namespace NKAPIService.API.System.Model
{
    /// <summary>
    /// 분석 서버에 설정된 모델의 Config
    /// </summary>
    public class ModelConfig
    {
        public bool Enable { get; set; }
        public DetectorModelType NetType { get; set; }
        [JsonConverter(typeof(SubNetTypeConverter))]
        public DetectorSubType NetSubType { get; set; }
        public DataType InferencePrecison { get; set; }
        public float DetectThresh { get; set; }
        public float NmsThresh { get; set; }
        public List<Label> Labels { get; set; }
        public List<ModelConfig> Options { get; set; }
        public string ModelWeightsPath { get; set; }
        public string CalibrationImagesPath { get; set; }
        public string TrtFileName { get; set; }
        public bool PrintDetectedElpasedTime { get; set; }
        public bool PrintDetectorFps { get; set; }
        public int NumberOfFrameDivisions { get; set; }
        public List<int> GpuIds { get; set; }
        /// <summary>
        /// 0: width, 1: height
        /// </summary>
        public List<int> InputSize { get; set; }
        public int BatchSize { get; set; }
        public bool Debug { get; set; }
    }
}
