using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;
using System;
using System.Threading.Tasks;

namespace NKAPISample.ViewModels
{
    public partial class ChannelViewModel : ObservableObject
    {

        private MainViewModel _mainVM;
       
        private string channelName = "NextK Channel";
        private string channelDescription = "725, 201 beon gil, Seocheon-ro, Giheung-gu, Yongin-si, Kyeonggi-do";
        private bool isAutoTimeout = false;
        private NKAPIService.API.Channel.Models.InputType channelType = NKAPIService.API.Channel.Models.InputType.SRC_IPCAM_NORMAL;
        private string channelGroupName = "NextK Group";

        public string NodeID { get => _mainVM.NodeID; set => _mainVM.NodeID = value; }
        public string ChannelID { get => _mainVM.ChannelID; set => _mainVM.ChannelID = value; }
        public string ChannelName { get => channelName; set => SetProperty(ref channelName, value); }
        public string ChannelDescription { get => channelDescription; set => SetProperty(ref channelDescription, value); }
        public bool IsAutoTimeout { get => isAutoTimeout; set => SetProperty(ref isAutoTimeout, value); }
        public string ChannelURL { get => _mainVM.ChannelURL; set => _mainVM.ChannelURL = value; }
        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }
        public string ChannelGroupName { get => channelGroupName; set => SetProperty(ref channelGroupName, value); }

        public ChannelViewModel(MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
        }


        public void CreateObject()
        {
            var channel = new RequestRegisterChannel()
            {
                NodeId = NodeID,
                ChannelId = "",
                InputType = channelType,
                GroupName = channelGroupName,
                Description = channelDescription,
                ChannelName = channelName,
                InputUrl = ChannelURL,
                InputUrlSub = ChannelURL,
                AutoTimeout = isAutoTimeout
            };

            SetPostURL(channel);
            SetRequestResult(channel);
            SetResponseResult(channel);
        }


        public void GetObject()
        {
            var requestChannel = new RequestGetChannel()
            {

            } as RequestGetChannel;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }

        public void RemoveObject()
        {
            var requestChannel = new RequestRemoveChannel()
            {

            } as RequestRemoveChannel;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }


        public void SetPostURL(IRequest channel)
        {
            _mainVM.PostURL = $"{_mainVM.HostURL}{channel.GetResource()}";
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="channel">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest channel)
        {
            if (channel is RequestCreateComputingNode createReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (channel is RequestGetComputingNode getReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (channel is RequestRemoveComputingNode removeReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
        }

        /// <summary>
        /// RequestCreateComputingNode Response Json String 결과값 세팅
        /// </summary>
        /// <param name="node">RequestCreateComputingNode 객체</param>
        /// <returns></returns>
        public async Task SetResponseResult(IRequest channel)
        {
            ResponseBase? response = await GetResponse(channel);
           
            if (response == null) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (channel is RequestRegisterChannel)
                {
                    var sampleNode = new ResponseRegisterChannel();
                    responseResult = $"No Response!! \n\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (channel is RequestGetChannel)
                {
                    var sampleNode = new ResponseGetComputingNode();
                    responseResult = $"No Response!! \n\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (channel is RequestRemoveChannel)
                {
                    var sampleNode = new ResponseRemoveChannel();
                    responseResult = $"No Response!! \n\nResponse sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }

                _mainVM.SetResponseResult(responseResult);
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    if (response is ResponseRegisterChannel createChannel)
                        _mainVM.ChannelID = createChannel.ChannelId;
                    else if (response is ResponseGetChannel getChannel)
                        _mainVM.ChannelID = getChannel.ChannelModel.ChannelId;
                    else if (response is ResponseRemoveChannel)
                        _mainVM.ChannelID = "";

                    _mainVM.SetResponseResult(responseResult);
                }
                else
                    _mainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }

        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            
            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));

            if (channel is RequestRegisterChannel createReq)
                return await service.Requset(createReq) as ResponseRegisterChannel;
            else if (channel is RequestGetChannel getReq)
                return await service.Requset(getReq) as ResponseGetChannel;
            else if (channel is RequestRemoveChannel removeReq)
                return await service.Requset(removeReq) as ResponseRemoveChannel;

            return null;
        }

    }
}
