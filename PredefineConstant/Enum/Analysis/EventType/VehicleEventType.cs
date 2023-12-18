namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum VehicleEventType
    {
        Unknown = 0,
        AllDetect = 1,
        Loitering = 2,
        Intrusion = 3,

        //AbnormalCongestion = 6,       // 영역 ROI 내 이동흐름 정체 (정상흐름 대비 상대적 정체도)

        IllegalParking = 8,            // 불법 주정차_10분 (1)
        AbnormalObjectCount = 9,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)

        TrafficActuatedSignal = 16,     // 차량 감응 신호

        TimeOccupancy = 17,             // 시간 점유율 (ITS)
        SpaceOccupancy = 18,            // 공간 점유율 (ITS)
        IntervalVelocity = 19,          // 구간 속도 (ITS)

        Headaway = 21,                  // 차두시간
        QueueLength = 22,               // 대기행렬 길이

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
    }
}
