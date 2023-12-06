using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using NKAPISample.Models;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.ComputingNode;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class ComputingNodeViewModel : ObservableObject
    {
        private MainViewModel _MainVM;
     

        private string _NodeName = "sample node";
        private string _License = "license-license-license-license";
        private DelegateCommand createCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;

        private string _HostIP;
        private string _HostPort;

        public string HostIP { get => _HostIP; set => SetProperty(ref _HostIP, value); }
        public string HostPort { get => _HostPort; set => SetProperty(ref _HostPort, value); }
        public string NodeName { get => _NodeName; set => SetProperty(ref _NodeName, value); }
        public string License { get => _License; set => SetProperty(ref _License, value); }
        public ICommand CreateCommand => createCommand ??= new DelegateCommand(CreateNode);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(GetNode);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(RemoveNode);



        public ComputingNodeViewModel()
        {
            _MainVM = Ioc.Default.GetRequiredService<MainViewModel>();
            HostIP = _MainVM.CurrentNode.HostIP;
            HostPort = _MainVM.CurrentNode.HostPort;
        }



        /// <summary>
        /// RestAPI 서버에 노드 객체 데이터 생성
        /// </summary>
        public void CreateNode()
        {
            _MainVM.SetResponseResult("Send Request [Create Node]");
            var requestNode = new RequestCreateComputingNode()
            {
                Host = _HostIP,
                NodeName = _NodeName,
                License = _License
            } as RequestCreateComputingNode;

            SetPostURL(requestNode);
            SetRequestResult(requestNode);
            SetResponseResult(requestNode);
        }


        /// <summary>
        /// RestAPI 서버를 통해 노드 정보 가져오기
        /// </summary>
        public void GetNode()
        {
            _MainVM.SetResponseResult("Send Request [Get Node]");
            var requestNode = new RequestGetComputingNode()
            {
                
            }; // 속성 없이 리스트로 받아옴

            SetPostURL(requestNode);
            SetRequestResult(requestNode);
            SetResponseResult(requestNode);
        }

        /// <summary>
        /// RestAPI 서버에서 노드 정보 지우기
        /// </summary>
        public void RemoveNode()
        {
            _MainVM.SetResponseResult("Send Request [Remove Node]");
            var requestNode = new RequestRemoveComputingNode()
            {
                NodeId= _MainVM.CurrentNode.NodeId
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
            _MainVM.SetPostURL($"{_MainVM.CurrentNode.HostURL}{node.GetResource()}");
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest node)
        {
            if (node is RequestCreateComputingNode createNode)
                _MainVM.RequestResult = JsonConvert.SerializeObject(createNode, Formatting.Indented);
            else if (node is RequestGetComputingNode getNode)
                _MainVM.RequestResult = JsonConvert.SerializeObject(getNode, Formatting.Indented);
            else if (node is RequestRemoveComputingNode removeNode)
                _MainVM.RequestResult = JsonConvert.SerializeObject(removeNode, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        public async Task SetResponseResult(IRequest node)
        {
            ErrorCode code = ErrorCode.REQUEST_TIMEOUT;
            ResponseBase? response = await GetResponse(node);

            if (response == null) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (node is RequestCreateComputingNode)
                {
                    var sampleNode = new ResponseCreateComputingNode();
                    responseResult = $"Error: NO RESPONSE\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if(node is RequestGetComputingNode)
                {
                    var sampleNode = new ResponseGetComputingNode();
                    responseResult = $"Error: NO RESPONSE\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if(node is RequestRemoveComputingNode)
                {
                    var sampleNode = new ResponseRemoveComputingNode();
                    responseResult = $"Error: NO RESPONSE\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }

                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                code = response.Code;
                if (code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    if (response is ResponseCreateComputingNode createNode)
                    {
                        _MainVM.UpdateNode(createNode.NodeId, HostIP, HostPort, NodeName, License);
                    }
                    else if (response is ResponseGetComputingNode getNode)
                    {
                        _MainVM.UpdateNode(getNode.Node.NodeId, HostIP, HostPort, NodeName, License);
                    }
                    else if (response is ResponseRemoveComputingNode)
                    {
                        _MainVM.ClearNode();
                    }

                    _MainVM.SetResponseResult(responseResult);
                }
                else
                    _MainVM.SetResponseResult($"[{code}] {response.Message}");
            }


        }

        
        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.CurrentNode.HostURL));

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
