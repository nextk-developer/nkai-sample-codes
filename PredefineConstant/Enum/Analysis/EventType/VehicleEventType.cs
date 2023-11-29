namespace PredefineConstant.Enum.Analysis.EventType
{
    public enum VehicleEventType
    {
        Unknown = 0,
        AllDetect = 1,
        Loitering = 2,
        // 영역 ROI 개체진입(침입)
        //Intrusion = 3,
        //RoiEnter = 4,
        IllegalParking = 11,            // 불법 주정차_10분 (1)
        AbnormalObjectCount = 12,       // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        //Longstay = 13,                // 영역 ROI 내 장시간 체류(주정차)
        LineEnter = 14,                 // Line ROI를 지나가는 객체감지 (Enter 방향)
        //LineExit = 15,                // Line ROI를 지나가는 객체감지 (Exit 방향)

        LineCrossing = 16,              // 양방향 라인 통과 (1)
        // 방향성 이동 카운트 (좌, 우, 유턴 등)
        Direction = 17,
        LeftTurn = 18,                  // 차량 좌회전
        RightTurn = 19,                 // 차량 우회전
        UTurn = 20,                     // 차량 유턴
        //CongestionIndex = 18,         // 사람 혼잡도 레벨 (1)
        //VehicleDensity = 19,          // 차량 밀도 (1)
        //StopVehicleCount = 20,        // 정차 중인 차량 수 카운트 (1)

        TrafficActuatedSignal = 26,     // 차량 감응 신호
        TimeOccupancy = 27,             // 시간 점유율 (ITS)
        SpaceOccupancy = 28,            // 공간 점유율 (ITS)
        IntervalVelocity = 29,          // 구간 속도 (ITS)

        Headaway = 31,                  // 차두시간
        QueueLength = 32,               // 대기행렬 길이

        Straight = 36,

        NonDetectionArea = 100,
    }
}
