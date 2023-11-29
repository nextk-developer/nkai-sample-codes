namespace NKAPIService.API.System
{
    public class RequestGetSystemStatus
    {
        public const string URI = "/v2/va/get-system-status";
    }

    public class ResponseGetSystemStaus
    {
        public int MyProperty { get; set; }
    }
}
