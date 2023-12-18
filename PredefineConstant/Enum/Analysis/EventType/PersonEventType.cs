namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum PersonEventType
    {
        Unknown = 0,
        AllDetect = 1,
        Loitering = 2,
        Intrusion = 3,

        //AbnormalPosture = 4,          // 자세이상(앉음, 쓰러짐)
        Falldown = 4,
        Violence = 5,                   // 싸움,폭력 (1) ??
        //AbnormalCongestion = 6,       // 영역 ROI 내 이동흐름 정체 (정상흐름 대비 상대적 정체도)
        //AbandonedObject = 7,

        AbnormalObjectCount = 9,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        Longstay = 10,                  // 영역 ROI 내 장시간 체류(주정차)

        ElderlyPeople = 20,

        PPE = 23,                       // LG 용접자 장구류 이벤트
        HeatTreatmentAccident = 24,
        CraneAccident = 25,
        //Weapon = 26,

        #region line event
        LineEnter = 100,                 // Line ROI를 지나가는 객체감지 (Enter 방향)
        //LineExit = 101,                // Line ROI를 지나가는 객체감지 (Exit 방향)
        LineCrossing = 102,              // 양방향 라인 통과 (1) 
        #endregion

        NonDetectionArea = 1000,
    }
}
