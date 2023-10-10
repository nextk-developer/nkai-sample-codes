using NKAPIService;

namespace NK_API_Test
{
    internal class TestBase
    {
        protected APIService service { get; }

        public TestBase()
        {
            var uri = new Uri($"http://127.0.0.1:8880");
            service = APIService.Build().SetUrl(uri);
        }
    }
}
