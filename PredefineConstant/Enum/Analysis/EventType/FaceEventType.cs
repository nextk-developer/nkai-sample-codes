﻿namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum FaceEventType
    {
        Unknown = 0,
        AllDetect = 1,
        MatchingFace = 13,              // 등록 얼굴 매칭 (1)
        NotWearingMask = 14,            // 얼굴 마스크 미착용 (1)
        NonDetectionArea = 1000,
    }
}
