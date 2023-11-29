namespace NKAPIService.API
{
    public enum ErrorCode
    {
        SUCCESS = 0,
        API_URI_PATH_ERROR = 10,//	api uri path 문제
        API_REQUEST_DATA_FORMAT_ERROR = 11,//11	api 요청 데이터 포맷 문제
        NOT_SUPPORT_FUNCTION = 12,//12	지원하지 않는 기능
        INVALID_LICENSE = 20,//20	라이선스 비활성화
        EXPIRED_LICENSE = 21,//21	라이선스 기간 만료
        NOT_SUPPORT_API_VERSION = 22,//22	지원하지 않는 API 버전
        RPC_PORT_ERROR = 23,//23	RPC 포트 문제
        HTTP_PORT_ERROR = 24,//24	HTTP 포트 문제
        CHANNEL_LICENSE_EXCEEDED = 25,
        WAITING_LOADING = 26,
        FAIL_CREATE_COMPUTING_NODE = 100,
        DIFFERENT_UID_COMPUTING_NODE = 101,
        FAIL_STREAMING_CAMERA = 200,
        FAIL_CREATE_CHANNEL = 201,
        FAIL_CALIBRATION = 210,
        FAIL_CREATE_SNAPSHOT = 220,
        FAIL_SETTING_ROI = 300,
        CUDA_BUG = 400,
        NODE_CONNECT_FAIL = 500,
        RTSP_FORMAT_NOT_CORRECT = 501,

        REQUEST_TIMEOUT,
        NOT_FOUND_COMPUTING_NODE,
        NOT_FOUND_ROI_ID,
        NOT_FOUND_CHANNEL_UID,
        NOT_FOUND_VIDEO,
        NOT_FOUND_STATISTICS,
        NOT_SUPPORT_VIDEO_EXT,
        FACE_DETECTOR_NOT_SET,

        DOES_NOT_MATCH_LINK_POINTS
    }
}
