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

        private string license = "license-license-license-license";
        public string License { get => license; set => SetProperty(ref license, value); }

        public ComputingNodeViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.CreateButtonClicked = delegate () {
                var node = createNode();
                var request = getRequest(node);
                var response = getResponse(node);
                return new object[2] { request, response };
            };
        }

        private string getRequest(RequestCreateComputingNode node)
        {
            return JsonConvert.SerializeObject(node);
        }

        private RequestCreateComputingNode createNode()
        {
            var request = new RequestCreateComputingNode()
            {
                Host = $"http://{hostURI}:{hostPort}",
                NodeName = nodeName,
                License = license
            } as RequestCreateComputingNode;

            return request;
        }

        private async Task<string> getResponse(RequestCreateComputingNode node)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            APIService service = APIService.Build().SetUrl(new Uri($"http://{hostURI}:{hostPort}"));
            var response = await service.Requset(node) as ResponseCreateComputingNode;
            if (response != null)
            {
                code = response.Code;
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response);
                    return responseResult;
                }
            }
            return "";


        }

    }
}
