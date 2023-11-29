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
using System.Threading.Channels;

namespace NKAPISample.ViewModels
{
    public partial class ComputingNodeViewModel : ObservableObject, ICommnunication
    {

        private string hostIP = "127.0.0.1";

        public string HostIP { get => hostIP; set => SetProperty(ref hostIP, value); }

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
        public string PostURL { get => postURI; set => SetProperty(ref postURI, value); }

        private string nodeID;
        public string NodeID { get => NodeID; set => SetProperty(ref nodeID, value); }

        private string channelID;
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }

        private string hostURL;
        public string HostURL { get => hostURL; set => SetProperty(ref hostURL, value); }

        public ComputingNodeViewModel(MainViewModel mainViewModel)
        {
            mainViewModel.PropertyChanged += MainViewModelPropertyChanged;
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = this.GetType().GetProperty(e.PropertyName);
            if (property == null)
                return;

            var main = sender as MainViewModel;
            var propertyValue = main.GetType().GetProperty(e.PropertyName).GetValue(main).ToString();
            property.SetValue(this, propertyValue);
        }



        /// <summary>
        /// RestAPI 서버에 노드 객체 데이터 생성
        /// </summary>
        public void CreateObject()
        {
            if (string.IsNullOrEmpty(HostURL))
                HostURL = $"http://{hostIP}:{hostPort}";

            var requestNode = new RequestCreateComputingNode()
            {
                Host = HostURL,
                NodeName = nodeName,
                License = license
            } as RequestCreateComputingNode;

            SetPostURL(requestNode);
            SetRequestResult(requestNode);
            SetResponseResult(requestNode);
        }


        /// <summary>
        /// RestAPI 서버를 통해 노드 정보 가져오기
        /// </summary>
        public void GetObject()
        {
            var requestNode = new RequestGetComputingNode()
            {

            } as RequestGetComputingNode;
            SetPostURL(requestNode);
            SetRequestResult(requestNode);
            SetResponseResult(requestNode);
        }

        /// <summary>
        /// RestAPI 서버에서 노드 정보 지우기
        /// </summary>
        public void RemoveObject()
        {
            var requestNode = new RequestRemoveComputingNode()
            {

            } as RequestRemoveComputingNode;
            SetPostURL(requestNode);
            SetRequestResult(requestNode);
            SetResponseResult(requestNode);
        }
        /// <summary>
        /// 노드 객체 생성 RestAPI POST URI 값 세팅
        /// </summary>
        /// <param name="node">생성된 RequestCreateComputingNode 객체</param>

        public void SetPostURL(IRequest node)
        {
            PostURL = $"{HostURL}{node.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest node)
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
        public async Task SetResponseResult(IRequest node)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            APIService service = APIService.Build().SetUrl(new Uri($"http://{hostIP}:{hostPort}"));
            ResponseBase? response = await GetResponse(node);

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

        
        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            
            APIService service = APIService.Build().SetUrl(new Uri(HostURL));

            if (channel is RequestCreateComputingNode createReq)
                return await service.Requset(createReq) as ResponseCreateComputingNode;
            else if (channel is RequestGetComputingNode getReq)
                return await service.Requset(getReq) as ResponseGetComputingNode;
            else if (channel is RequestRemoveComputingNode removeReq)
                return await service.Requset(removeReq) as ResponseRemoveComputingNode;

            return null;
        }

    }
}
