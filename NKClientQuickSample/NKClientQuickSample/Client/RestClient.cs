using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NKClientQuickSample.Client
{

    public class RestClient
    {
        public event EventHandler<string> ResponseAPIHandler;
        public event EventHandler<string> ResponseDirectDrawHandler;
        public event EventHandler<string> ResponseUidHandler;
        public void RequestTo(string baseURI, string json, bool useResponse)
        {
            Task.Run(() =>
            {
                try
                {
                    var splitUris = baseURI.Split('/');
                    if (splitUris.Length > 3)
                    {
                        var pathBuilder = new StringBuilder();
                        for (int i = 3; i < splitUris.Length; i++)
                        {
                            pathBuilder.Append($"/{splitUris[i]}");
                        }
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseURI);
                            client.Timeout = new TimeSpan(0, 0, 0, 3);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var messge = client.PostAsync(pathBuilder.ToString(), new StringContent(json, UTF8Encoding.UTF8, "application/json")).Result;
                            if (messge.IsSuccessStatusCode)
                            {
                                string res = messge.Content.ReadAsStringAsync().Result;

                                if (useResponse == false)
                                    ResponseAPIHandler?.Invoke(this, res);

                                //UI에서 최근 Node Id, Channel Id 확인을 위함
                                switch (splitUris[splitUris.Length - 1])
                                {
                                    case "create-computing-node":
                                        {
                                            Test.ResponseCompute compute = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseCompute>(res);
                                            ResponseUidHandler?.Invoke("nodeId", compute.nodeId);
                                        }
                                        break;
                                    case "register-channel":
                                        {
                                            Test.ResponseChannel channel = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseChannel>(res);
                                            ResponseUidHandler?.Invoke("channelId", channel.channelId);
                                        }
                                        break;
                                    case "create-roi":
                                        {
                                            Test.ResponseRoiInfo roi = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseRoiInfo>(res);
                                            ResponseUidHandler?.Invoke("roiId", roi.roiId);
                                        }
                                        break;
                                    case "list-roi":
                                        {
                                            if(useResponse)
                                                ResponseDirectDrawHandler?.Invoke(this, res);
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ResponseAPIHandler?.Invoke(this, "Exeption Error");
                }
            });
        }
        public string GetRequestAsync(string baseURI, string json)
        {
            string response = null;
            string[] splitUris = baseURI.Split('/');
            int uriLength = splitUris.Length;
            if (uriLength > 3)
            {
                StringBuilder pathBuilder = new StringBuilder();
                for (int i = 3; i < uriLength; i++)
                {
                    pathBuilder.Append($"/{splitUris[i]}");
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURI);
                    client.Timeout = new TimeSpan(0, 0, 0, 3);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage messge = client.PostAsync(pathBuilder.ToString(), new StringContent(json, UTF8Encoding.UTF8, "application/json")).Result;
                    if (messge.IsSuccessStatusCode)
                    {
                        response = messge.Content.ReadAsStringAsync().Result;
                    }
                }
            }
            return response;
        }
    }
}
