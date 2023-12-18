namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum IntegrationEventType
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

        IllegalParking = 8,            // 불법 주정차_10분 (1)
        AbnormalObjectCount = 9,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        Longstay = 10,                  // 영역 ROI 내 장시간 체류(주정차)

        Smoke = 11,                     // 화재 연기 (1)
        Flame = 12,                     // 화재 불꽃 (1)
        MatchingFace = 13,              // 등록 얼굴 매칭 (1)
        NotWearingMask = 14,            // 얼굴 마스크 미착용 (1)
        NoHelmet = 15,                  // 헬멧 미착용
        TrafficActuatedSignal = 16,     // 차량 감응 신호

        TimeOccupancy = 17,             // 시간 점유율 (ITS)
        SpaceOccupancy = 18,            // 공간 점유율 (ITS)
        IntervalVelocity = 19,          // 구간 속도 (ITS)
        ElderlyPeople = 20,

        Headaway = 21,                  // 차두시간
        QueueLength = 22,               // 대기행렬 길이
        PPE = 23,                       // LG 용접자 장구류 이벤트
        HeatTreatmentAccident = 24,
        CraneAccident = 25,
        //Weapon = 26,

        #region line event
        LineEnter = 100,                 // Line ROI를 지나가는 객체감지 (Enter 방향)
        //LineExit = 101,                // Line ROI를 지나가는 객체감지 (Exit 방향)
        LineCrossing = 102,              // 양방향 라인 통과 (1) 
        LineStraight = 103,                // 차량 직진
        LineLeftTurn = 104,                // 차량 좌회전
        LineRightTurn = 105,               // 차량 우회전
        LineUTurn = 106,                   // 차량 유턴
        #endregion

        #region multi line evnet
        Direction = 200,
        Straight = 201,
        LeftTurn = 202,                  // 차량 좌회전
        RightTurn = 203,                 // 차량 우회전
        UTurn = 204,                     // 차량 유턴 
        #endregion



        NonDetectionArea = 1000,
        TidyUpWorkbench = 1001,
        Leak = 1002,
        FloodedOrSnowRoad = 1003,

        // 내부 이벤트,,사용자가 설정하지 않음
        ITS,
    }


    public static class IntegrationEventTypeEx
    {
        public static DrawingType ToDrawingType(this IntegrationEventType eventType)
        {
            switch (eventType)
            {
                case IntegrationEventType.FloodedOrSnowRoad:
                    return DrawingType.MultiPolygon;
                //line
                case IntegrationEventType.LineCrossing:
                case IntegrationEventType.LineEnter:
                case IntegrationEventType.LineStraight:
                case IntegrationEventType.LineRightTurn:
                case IntegrationEventType.LineLeftTurn:
                case IntegrationEventType.LineUTurn:
                    return DrawingType.Line;
                //multiline
                case IntegrationEventType.Direction:
                case IntegrationEventType.Straight:
                case IntegrationEventType.LeftTurn:
                case IntegrationEventType.RightTurn:
                case IntegrationEventType.UTurn:
                case IntegrationEventType.IntervalVelocity:
                    return DrawingType.MultiLine;
                default:
                    return DrawingType.Rect;
            }
        }
    }
}