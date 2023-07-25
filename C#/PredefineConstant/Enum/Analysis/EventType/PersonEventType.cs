namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum PersonEventType
    {
        Unknown = 0,
        AllDetect = 1,
        Loitering = 2,
        // 영역 ROI 개체진입(침입)
        Intrusion = 3,
        //RoiEnter = 4,

        //AbnormalPosture = 5,          // 자세이상(앉음, 쓰러짐)
        Falldown = 6,
        Violence = 7,                   // 싸움,폭력 (1) ??
            
        //RoiExit = 8,                  // 영역 ROI 개체진출(사라짐)

        //AbnormalCongestion = 9,       // 영역 ROI 내 이동흐름 정체 (정상흐름 대비 상대적 정체도)
        //AbandonedObject = 10,

        AbnormalObjectCount = 12,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        Longstay = 13,                  // 영역 ROI 내 장시간 체류(주정차)
        LineEnter = 14,                 // Line ROI를 지나가는 객체감지 (Enter 방향)
        //LineExit = 15,                // Line ROI를 지나가는 객체감지 (Exit 방향)

        LineCrossing = 16,               // 양방향 라인 통과 (1)

        ElderlyPeople = 30,
        NonDetectionArea = 100,
    }
}
