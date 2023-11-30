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
    public partial class ComputingNodeViewModel : ObservableObject
    {
        private MainViewModel _mainVM;
        private string nodeName = "sample node";
        private string license = "license-license-license-license";

        public string HostIP { get => _mainVM.HostIP; set => _mainVM.HostIP = value; }
        public string HostPort { get => _mainVM.HostPort; set => _mainVM.HostPort = value; }
        public string NodeName { get => nodeName; set => SetProperty(ref nodeName, value); }
        public string License { get => license; set => SetProperty(ref license, value); }



        public ComputingNodeViewModel(MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
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
            var requestNode = new RequestCreateComputingNode()
            {
                Host = _mainVM.HostURL,
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
            _mainVM.PostURL = $"{_mainVM.HostURL}{node.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest node)
        {
            if (node is RequestCreateComputingNode createNode)
                _mainVM.RequestResult = JsonConvert.SerializeObject(createNode, Formatting.Indented);
            else if (node is RequestGetComputingNode getNode)
                _mainVM.RequestResult = JsonConvert.SerializeObject(getNode, Formatting.Indented);
            else if (node is RequestRemoveComputingNode removeNode)
                _mainVM.RequestResult = JsonConvert.SerializeObject(removeNode, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        public async Task SetResponseResult(IRequest node)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));
            ResponseBase? response = await GetResponse(node);

            if (response == null)
                _mainVM.ResponseResult = "No Response";
            else
            {
                code = response.Code;
                if (code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    _mainVM.ResponseResult = responseResult;
                }
                else
                    _mainVM.ResponseResult = $"[{code}] {response.Message}";
            }


        }

        
        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            
            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));

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
