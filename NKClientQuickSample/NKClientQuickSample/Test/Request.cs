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
        EVT_LOITERING = 1,              //배회
        EVT_INTRUSION,                  //침입
        EVT_QUEUEING,                   //대기열
        EVT_ABNORMAL_CONGESTION,        //정체 (정상 흐름 대비)
        EVT_ABNORMAL_OBJ_COUNT,         //정체 (영역 내 설정된 객체 수)
        EVT_ROI_COUNT,                  //ROI 카운팅
        EVT_LINE_COUNT,                 //LINE 카운팅
        EVT_ILLEGAL_PARKING,            //불법 주정차
        EVT_WRONG_WAY,                  //역주행
        EVT_DIRECTION_COUNTING,         //방향성 이동
        EVT_VEHICLE_SPEED,              //속도 (차량)
        EVT_VEHICLE_DENSITY,            //밀도 (차량) _ 밀도에 대한 기준은?
        EVT_STOP_VEHICLE_COUNTING,      //정지 카운팅 (차량)
        EVT_SIGNAL_WAITING_TIME,        //신호 대기 시간
        EVT_PARKING_SPACE,              //주차 공간 점유 검출
        EVT_CROSSWALK_QUEUEING          //횡단보도 대기열 카운팅
    }
}
