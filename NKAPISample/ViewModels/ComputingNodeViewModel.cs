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
using System.Text.Json;
using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;

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

        private string requestResult;
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }

        private string responseResult;
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }

        private string postURI;
        public string PostURI { get => postURI; set => SetProperty(ref postURI, value); }



        public ComputingNodeViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.CreateButtonClicked = createNode;
            mainViewModel.GetButtonClicked = getNode;
            mainViewModel.RemoveButtonClicked = removeNode;
        }



        /// <summary>
        /// RestAPI 서버에 노드 객체 데이터 생성
        /// </summary>
        private void createNode()
        {
            var requestNode = new RequestCreateComputingNode()
            {
                Host = $"http://{hostURI}:{hostPort}",
                NodeName = nodeName,
                License = license
            } as RequestCreateComputingNode;

            setPostURI(requestNode);
            setRequestResult(requestNode);
            setResponseResult(requestNode);
        }


        /// <summary>
        /// RestAPI 서버를 통해 노드 정보 가져오기
        /// </summary>
        private void getNode()
        {
            var requestNode = new RequestGetComputingNode()
            {

            } as RequestGetComputingNode;
        }

        /// <summary>
        /// RestAPI 서버에서 노드 정보 지우기
        /// </summary>
        private void removeNode()
        {
            var requestNode = new RequestRemoveComputingNode()
            {

            } as RequestRemoveComputingNode;
        }
        /// <summary>
        /// 노드 객체 생성 RestAPI POST URI 값 세팅
        /// </summary>
        /// <param name="node">생성된 RequestCreateComputingNode 객체</param>

        private void setPostURI(IRequest node)
        {
            string host = $"http://{hostURI}:{hostPort}";
            PostURI = $"{host}{node.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        private void setRequestResult(IRequest node)
        {
            if (node is RequestCreateComputingNode createNode)
                RequestResult = JsonConvert.SerializeObject(createNode, Formatting.Indented);
            else if (node is RequestGetComputingNode getNode)
                RequestResult = JsonConvert.SerializeObject(getNode, Formatting.Indented);
            else if (node is RequestRemoveComputingNode removeNode)
                RequestResult = JsonConvert.SerializeObject(removeNode, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        private async Task setResponseResult(RequestCreateComputingNode node)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            APIService service = APIService.Build().SetUrl(new Uri($"http://{hostURI}:{hostPort}"));
            var response = await service.Requset(node) as ResponseCreateComputingNode;
            
            if (response != null)
            {
                code = response.Code;
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    ResponseResult = responseResult;
                }
            }
        }

    }
}
