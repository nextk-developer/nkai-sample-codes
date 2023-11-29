namespace NKAPIService.API.System.Model
{
    public enum DetectorModelType
    {
        YOLOV8 = 0,
        YOLOV4 = 1,
        YOLOV5 = 2,
        YUNET = 3,
        RETINA_MOBILE_NET = 4,
        RETINA_COV = 5,
        RETINA_R50 = 6,
        RETINA_R100 = 7,
        DEEPSPARSE = 8,
        YOLOV5_ONNX = 9,

        //option
        ARC_MOBILE_NET = 100,
        ARC_R50 = 101,
        ARC_R100 = 102,
        ALPHA_POSE = 103,
        MASK_CLASSIFICATION = 104,
        LPR_NET = 105,
        FAST_REID = 106,
        YOLOV5_CLS = 107,
        PPE_CLASSIFICATION = 108,
    };

    public enum DetectorSubType { None, N, S, M, L, X }

}