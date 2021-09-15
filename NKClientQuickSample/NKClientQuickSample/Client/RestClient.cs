﻿using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NKClientQuickSample.Client
{

    public class RestClient
    {
        public event EventHandler<string> ResponseAPIHandler;
        public event EventHandler<string> ResponseUidHandler;
        public void RequestTo(string baseURI, string json)
        {
            Task.Run(() =>
            {
                try
                {
                    var splitUris = baseURI.Split('/');
                    var pathBuilder = new StringBuilder();
                    if (splitUris.Length > 3)
                    {
                        for (int i = 3; i < splitUris.Length; i++)
                        {
                            pathBuilder.Append($"/{splitUris[i]}");
                        }
                        string path = pathBuilder.ToString();

                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(baseURI);
                            client.Timeout = new TimeSpan(0, 0, 0, 10);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpResponseMessage messge = client.PostAsync(path, new StringContent(json, UTF8Encoding.UTF8, "application/json")).Result;
                            if (messge.IsSuccessStatusCode)
                            {
                                string resString = messge.Content.ReadAsStringAsync().Result;
                                ResponseAPIHandler?.Invoke(this, resString);

                                //UI에서 최근 Node Id, Channel Id 확인을 위함
                                switch (splitUris[splitUris.Length - 1])
                                {
                                    case "create-computing-node":
                                        {
                                            Test.ResponseCompute compute = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseCompute>(resString);
                                            ResponseUidHandler?.Invoke("nodeId", compute.nodeId);
                                        }
                                        break;
                                    case "register-channel":
                                        {
                                            Test.ResponseChannel channel = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseChannel>(resString);
                                            ResponseUidHandler?.Invoke("channelId", channel.channelId);
                                        }
                                        break;
                                    case "create-roi":
                                        {
                                            Test.ResponseRoiInfo roi = Newtonsoft.Json.JsonConvert.DeserializeObject<Test.ResponseRoiInfo>(resString);
                                            ResponseUidHandler?.Invoke("roiId", roi.roiId);
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
            StringBuilder pathBuilder = new StringBuilder();
            if (splitUris.Length > 3)
            {
                for (int i = 3; i < splitUris.Length; i++)
                {
                    pathBuilder.Append($"/{splitUris[i]}");
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURI);
                    client.Timeout = new TimeSpan(0, 0, 0, 5);
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
