using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NKAPISample.Models;
using NKAPISample.Properties;
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
        private MainViewModel _MainVM;
        private DelegateCommand createCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;
        private string _ChannelURL;

        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }
        public ICommand CreateCommand => createCommand ??= new DelegateCommand(CreateChannel);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(GetChannel);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(RemoveChannel);

        public string ChannelURL { get => _ChannelURL; set => SetProperty(ref _ChannelURL, value); }
        public ChannelViewModel(MainViewModel mainViewModel, ChannelModel channel)
        {
            _MainVM = mainViewModel;
            ChannelURL = channel.InputUrl;
        }


        public void CreateChannel()
        {
            _MainVM.SetResponseResult("Send Request [Create Channel]");
            var channel = new RequestRegisterChannel()
            {
                NodeId = _MainVM.Node.NodeId,
                ChannelId = "",
                InputType = channelType,
                GroupName = "NextK Group",
                Description = "NextK",
                ChannelName = "NextK Channel",
                InputUrl = ChannelURL,
                InputUrlSub = ChannelURL,
                AutoTimeout = true
            };

            SetPostURL(channel);
            SetRequestResult(channel);
            SetResponseResult(channel);
        }


        public void GetChannel()
        {
            _MainVM.SetResponseResult("Send Request [Get Channel]");
            var requestChannel = new RequestListChannels()
            {
                NodeId = _MainVM.Node.NodeId
            } as RequestListChannels;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }

        public void RemoveChannel()
        {
            _MainVM.SetResponseResult("Send Request [Remove Channel]");
            var requestChannel = new RequestRemoveChannel()
            {
                NodeId = _MainVM.Node.NodeId,
                ChannelId = _MainVM.Channel.ChannelUid
            } as RequestRemoveChannel;
            SetPostURL(requestChannel);
            SetRequestResult(requestChannel);
            SetResponseResult(requestChannel);
        }


        public void SetPostURL(IRequest channel)
        {
            _MainVM.SetPostURL($"{_MainVM.Node.HostURL}{channel.GetResource()}");
        }


        /// <summary>
        /// RequestCreateComputingNode Json String 값 세팅
        /// </summary>
        /// <param name="channel">RequestCreateComputingNode 객체</param>
        public void SetRequestResult(IRequest channel)
        {
            if (channel is RequestRegisterChannel createReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (channel is RequestListChannels getReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (channel is RequestRemoveChannel removeReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
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
                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    if (response is ResponseRegisterChannel createChannel)
                    {
                        _MainVM.UpdateChannel(createChannel.ChannelId, createChannel.MediaServerUrl, createChannel.MediaServerUrlSub);
                    }
                    else if (response is ResponseListChannels listChannel)
                    {
                        var item = listChannel.ChannelItems.FindLast(ch => !string.IsNullOrEmpty(ch.ChannelId));
                        if (item != null)
                        {
                            _MainVM.UpdateChannel(item.ChannelId, item.MediaServerUrl, item.MediaServerUrlSub);
                        }
                    }
                    else if (response is ResponseRemoveChannel)
                        _MainVM.ClearChannel();

                    _MainVM.SetResponseResult(responseResult);
                }
                else
                    _MainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }

        public async Task<ResponseBase> GetResponse(IRequest channel)
        {
            
            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.Node.HostURL));

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
