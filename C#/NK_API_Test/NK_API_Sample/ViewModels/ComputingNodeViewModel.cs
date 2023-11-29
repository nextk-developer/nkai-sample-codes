using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using NKAPIService.API.ComputingNode;
using NKAPIService.API;
using System;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using NKAPIService;
using DevExpress.XtraPrinting.Native.Properties;
using RestSharp.Authenticators;
using RestSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using DevExpress.Pdf.Native.BouncyCastle.Ocsp;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NKAPISample.ViewModels
{
    public partial class ComputingNodeViewModel : ObservableObject
    {

        private string hostURI = "127.0.0.1";

        public string HostURI { get => hostURI; set => SetProperty(ref hostURI, value); }

        private string hostPort = "8880";

        public string HostPort { get => hostPort; set => SetProperty(ref hostPort, value); }

        private string nodeName = "sample node";

        public string NodeName { get => nodeName; set => SetProperty(ref nodeName, value); }

        private string license = "4/Hpx/q3jP42zD1RXm2I4ya2XSYLORbgYRDhA8fOGqld3bEsHNHIp8EstNUlvKhS";
        public string License { get => license; set => SetProperty(ref license, value); }

        public ComputingNodeViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.CreateButtonClicked = delegate () { return getCreateRequest().Result; };
        }


        private async Task<string> getCreateRequest()
        {

            var request = new RequestCreateComputingNode()
            {
                Host = $"http://{hostURI}:{hostPort}",
                NodeName = nodeName,
                License = license
            } as RequestCreateComputingNode;

            var req = new RestRequest()
            {
                Resource = request.GetResource(),
                Method = Method.Post,
                RequestFormat = DataFormat.Json,
            };

            var body = JsonConvert.SerializeObject(request);
            req.AddHeader("Content-type", "application/json; charset=utf-8");
            req.AddHeader("Accept-Encoding", "gzip");
            req.AddJsonBody(body);

            RestClient restClient = new RestClient(new RestClientOptions() { BaseUrl = new Uri($"http://{hostURI}:{hostPort}") });

            var res = await restClient.ExecutePostAsync(req);
            if (res.StatusCode == System.Net.HttpStatusCode.OK)
                return res.Content;

            return "";
        }

    }
}
