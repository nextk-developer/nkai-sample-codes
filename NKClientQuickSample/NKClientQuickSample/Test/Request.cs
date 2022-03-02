using System;
using System.Collections.Generic;
using System.Text;

namespace NKClientQuickSample.Test
{
    public class RequestAddCompute
    {
        public string host { get; set; }
        public int httpPort { get; set; }
        public string nodeName { get; set; }
        public string license { get; set; }
    }
    public class RequestCompute
    {
        public string nodeId { get; set; }
    }

    public class RequestAddChannel
    {
        public string nodeId { get; set; }
        public string channelName { get; set; }
        public string inputUri { get; set; }
        public string inputType { get; set; }
    }
    public class RequestChannel
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
    }
    public class RequestAddROI
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public EventType eventType { get; set; }
        public string roiName { get; set; }
        public string description { get; set; }
        public List<RoiDot> roiDots { get; set; }
        public Filter EventFilter { get; set; }
    }
    public class Filter
    {
        public int minDetectSize { get; set; }
        public int maxDetectSize { get; set; }
        public List<ObjectType> objectsTarget { get; set; }
    }
    public enum ObjectType
    {
        PERSON = 0,                   // 사람
        BIKE = 1,                     // 자전거
        CAR = 2,                      // 승용차
        MOTORCYCLE = 3,               // 오토바이
        BUS = 4,                      // 버스
        TRUCK = 5,                    // 트럭
        EXCAVATOR = 6,                // 굴착기
        TANKTRUCK = 7,                // 탱크트럭
        FORKLIFT = 8,                 // 지게차
        LEMICON = 9,                  // 레미콘
        CULTIVATOR = 10,              // 경운기
        TRACTOR = 11,                 // 트랙터
        SMOKE = 50,                   // 불꽃
        FLAME = 51,                   // 연기
        FACE_MAN = 200,               // 얼굴_남자
        FACE_WOMAN = 201              // 얼굴_여자
    }
    public class RoiDot
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class RequestROI
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public string roiId { get; set; }
    }
    public class RequestLink
    {
        public string nodeId { get; set; }
        public string channelId { get; set; }
        public List<string> roiLink { get; set; }
    }
    public class RequestVaStart
    {
        public string nodeId { get; set; }
        public List<string> channelIds { get; set; }
        public VAOperations operation { get; set; }
    }
    public enum VAOperations
    {
        VA_START,
        VA_STOP,
        VA_RESET,
    }
    public enum EventType
    {
        EVT_LONGSTAY,   // 장기체류
        EVT_LOITERING,  // 배회
        EVT_INTRUSION,  // 침입
        EVT_FALLDOWN,   // 쓰러짐
        EVT_ABANDONMENT,        // 유기 (미구현)
        EVT_VIOLENCE,           // 싸움
        EVT_PEOPLE_COUNTING_A,  // 출입자 카운팅 영역 A (KISA)
        EVT_PEOPLE_COUNTING_B,  // 출입자 카운팅 영역 B (KISA)
        EVT_PEOPLE_COUNTING,    // 출입자 카운팅 (KISA)
        EVT_QUEUEING,                   // 대기열 (KISA)
        EVT_ABNORMAL_CONGESTION,       // 영역 ROI 내 이동흐름 정체 (정상흐름 대비 상대적 정체도)
        EVT_ABNORMAL_OBJ_COUNT,        // 영역 ROI 내 개체밀집 (정의된 개체수 이상의 객체 존재)
        EVT_ROI_COUNT,                 // 영역 ROI 카운팅
        EVT_LINE_COUNT,                // Line ROI 카운팅
        EVT_ILLEGAL_PARKING,           // 불법 주정차
        EVT_WRONG_WAY,                 // Line ROI 역주행
        EVT_DIRECTION_COUNTING,        // 방향성 이동(직전, 좌/우회전, 유턴) 카운팅
        EVT_PEOPLE_CONGESTION_LEVEL,   // 영역내 혼잡도 (상,중,하)
        EVT_VEHICLE_SPEED,             // 차량 속도
        EVT_VEHICLE_DENSITY,           // 차량 밀도
        EVT_STOP_VEHICLE_COUNTING,     // 정지 차량 카운팅
        EVT_SIGNAL_WAITING_TIME,       // 신호 대기 시간
        EVT_ANOMALY_KISA,              // 이상 징후 (KISA 방위)
        EVT_LINE_ENTER,                // Line ROI In 카운팅
        EVT_LINE_EXIT,                 // Line ROI Out 카운팅
        EVT_TYPE_SIZE,
        EVT_FIRE,
        EVT_FACE_MATCHING,
        EVT_FACE_MASKED,
    }
}
