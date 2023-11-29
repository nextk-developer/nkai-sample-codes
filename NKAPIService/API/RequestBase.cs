namespace NKAPIService.API
{
    public class RequestBase
    {
        public readonly string URI;

        public RequestBase(string uri)
        {
            this.URI = uri;
        }
    }
}
