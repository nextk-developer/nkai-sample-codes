using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class ChannelViewModel : ObservableObject
    {
        private NKAPIService.API.Channel.Models.InputType channelType = 0;
        private MainViewModel _mainVM;
        private DelegateCommand createCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;

        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }
        public ICommand CreateCommand => createCommand ??= new DelegateCommand(CreateChannel);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(GetChannel);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(RemoveChannel);

        public ChannelViewModel(MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
        }


        public void CreateChannel()
        {
            _mainVM.SetResponseResult("Send Request [Create Channel]");
            var channel = new RequestRegisterChannel()
            {
                NodeId = _mainVM.NodeID,
                ChannelId = "",
                InputType = channelType,
                GroupName = "NextK Group",
                Description = "NextK",
                ChannelName = "NextK Channel",
                InputUrl = _mainVM.ChannelURL,
                InputUrlSub = _mainVM.ChannelURL,
                AutoTimeout = true
            };

            SetPostURL(channel);
            SetRequestResult(channel);
            SetResponseResult(channel);
        }


        public void GetChannel()
        {
            _mainVM.SetResponseResult("Send Request [Get Channel]");
            var requestChannel = new RequestListChannels()
            {
                NodeId = _mainVM.NodeID
            } as RequestListChannels;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }

        public void RemoveChannel()
        {
            _mainVM.SetResponseResult("Send Request [Remove Channel]");
            var requestChannel = new RequestRemoveChannel()
            {
                NodeId = _mainVM.NodeID,
                ChannelId = _mainVM.ChannelID
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
            if (channel is RequestRegisterChannel createReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (channel is RequestListChannels getReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (channel is RequestRemoveChannel removeReq)
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
           
            if (response == null || response.Code != ErrorCode.SUCCESS) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (response == null)
                    responseResult = "Error: NO RESPONSE\n";
                else
                    responseResult = $"Error: {response.Code}\n";

                
                if (channel is RequestRegisterChannel)
                {
                    var sampleNode = new ResponseRegisterChannel();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (channel is RequestListChannels)
                {
                    var sampleNode = new ResponseListChannels();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (channel is RequestRemoveChannel)
                {
                    var sampleNode = new ResponseRemoveChannel();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                _mainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    if (response is ResponseRegisterChannel createChannel)
                    {
                        _mainVM.ChannelID = createChannel.ChannelId;
                    }
                    else if (response is ResponseListChannels listChannel)
                    {
                        var item = listChannel.ChannelItems.FindLast(ch => !string.IsNullOrEmpty(ch.ChannelId));
                        if (item != null)
                            _mainVM.ChannelID = item.ChannelId;
                    }
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
            else if (channel is RequestListChannels getReq)
                return await service.Requset(getReq) as ResponseListChannels;
            else if (channel is RequestRemoveChannel removeReq)
                return await service.Requset(removeReq) as ResponseRemoveChannel;

            return null;
        }

    }
}
