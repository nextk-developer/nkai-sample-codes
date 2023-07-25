namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum IntegrationEventType
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

        IllegalParking = 11,            // 불법 주정차_10분 (1)
        AbnormalObjectCount = 12,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        Longstay = 13,                  // 영역 ROI 내 장시간 체류(주정차)
        LineEnter = 14,                 // Line ROI를 지나가는 객체감지 (Enter 방향)
        //LineExit = 15,                // Line ROI를 지나가는 객체감지 (Exit 방향)

        LineCrossing = 16,              // 양방향 라인 통과 (1)
        // 방향성 이동 카운트 (좌, 우, 유턴 등)
        Direction = 17,
        LeftTurn = 18,                  // 차량 좌회전
        RightTurn = 19,                 // 차량 우회전
        UTurn = 20,                     // 차량 유턴

        Smoke = 21,                     // 화재 연기 (1)
        Flame = 22,                     // 화재 불꽃 (1)
        MatchingFace = 23,              // 등록 얼굴 매칭 (1)
        NotWearingMask = 24,            // 얼굴 마스크 미착용 (1)
        NoHelmet = 25,                  // 헬멧 미착용
        TrafficActuatedSignal = 26,     // 차량 감응 신호

        TimeOccupancy = 27,             // 시간 점유율 (ITS)
        SpaceOccupancy = 28,            // 공간 점유율 (ITS)
        IntervalVelocity = 29,          // 구간 속도 (ITS)
        ElderlyPeople = 30,

        Headaway = 31,                  // 차두시간
        QueueLength = 32,               // 대기행렬 길이

        //Weapon = 27,
        NonDetectionArea = 100,
    }
}