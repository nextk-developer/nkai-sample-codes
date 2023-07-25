using System;
using System.Net;
using System.Threading.Tasks;

namespace Utils.Extentions
{
    public static class WebRequestExtensions
    {
        public static Task<WebResponse> GetResponseAsync(this WebRequest request, TimeSpan timeout)
        {
            return Task.Factory.StartNew<WebResponse>(() =>
            {
                var t = Task.Factory.FromAsync<WebResponse>(
                        request.BeginGetResponse,
                        request.EndGetResponse,
                        null);
                try
                {
                    if (!t.Wait(timeout)) throw new TimeoutException();
                }
                catch (Exception e)
                {
                    //throw new TimeoutException();
                    return null;
                }

                return t.Result;
            });
        }
    }
}
