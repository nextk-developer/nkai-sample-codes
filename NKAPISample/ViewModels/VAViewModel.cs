using NKAPIService.API.VideoAnalysisSetting.Models;
using NKAPIService.API.VideoAnalysisSetting;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Channels;
using FFmpeg.AutoGen;
using NKAPIService.API;
using Newtonsoft.Json;
using System.Threading.Tasks;
using NKAPIService.API.Channel;
using NKAPIService;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace NKAPISample.ViewModels
{
    public partial class VAViewModel
    {
        private MainViewModel _MainVM;
        private RelayCommand startCommand;
        private RelayCommand resetCommand;
        private RelayCommand stopCommand;
        public IRelayCommand StartCommand => startCommand ??= new RelayCommand(StartVA);
        public IRelayCommand ResetCommand => resetCommand ??= new RelayCommand(ResetVA);
        public IRelayCommand StopCommand => stopCommand ??= new RelayCommand(StopVA);
        public VAViewModel()
        {
            _MainVM = Ioc.Default.GetRequiredService<MainViewModel>();
        }

        internal void StartVA()
        {
            _MainVM.SetResponseResult("Send Request [Start VA]");
            var req = new RequestControl()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelIDs = new List<string>() { _MainVM.CurrentNode.CurrentChannel.ChannelUid },
                Operation = Operations.VA_START
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void ResetVA()
        {
            _MainVM.SetResponseResult("Send Request [Reset VA]");
            var req = new RequestControl()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelIDs = new List<string>() { _MainVM.CurrentNode.CurrentChannel.ChannelUid },
                Operation = Operations.VA_RST
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void StopVA()
        {
            _MainVM.SetResponseResult("Send Request [Stop VA]");
            var req = new RequestControl()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelIDs = new List<string>() { _MainVM.CurrentNode.CurrentChannel.ChannelUid },
                Operation = Operations.VA_STOP
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }


        public void SetPostURL(IRequest req)
        {
            _MainVM.SetPostURL($"{_MainVM.CurrentNode.HostURL}{req.GetResource()}");
        }

        public void SetRequestResult(IRequest req)
        {
            _MainVM.RequestResult = JsonConvert.SerializeObject(req, Formatting.Indented);
        }

        public async Task SetResponseResult(IRequest req)
        {
            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.CurrentNode.HostURL));
            ResponseBase? response =  await service.Requset(req) as ResponseControl;

            if (response == null || response.Code != ErrorCode.SUCCESS) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (string.IsNullOrEmpty(_MainVM.CurrentNode.NodeId))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_COMPUTING_NODE}\n";
                else if (string.IsNullOrEmpty(_MainVM.CurrentNode.CurrentChannel.ChannelUid))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_CHANNEL_UID}\n";
                else if (string.IsNullOrEmpty(_MainVM.GetCurrentRoiID()))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_ROI_ID}\n";
                else if (response == null)
                    responseResult = "Error: NO RESPONSE\n";

                var sampleNode = new ResponseControl();
                responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";

                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    _MainVM.SetResponseResult(responseResult);

                    RequestControl request = req as RequestControl;
                    if (request != null && request.Operation == Operations.VA_START)
                        _MainVM.VAStarted?.Invoke(service);
                    else if (request != null && request.Operation == Operations.VA_STOP)
                        _MainVM.VAStopped?.Invoke();
                }
                else
                    _MainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }


    }
}
