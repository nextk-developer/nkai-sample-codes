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

namespace NKAPISample.ViewModels
{
    public partial class VAViewModel
    {
        private MainViewModel _mainVM;
        private DelegateCommand startCommand;
        private DelegateCommand resetCommand;
        private DelegateCommand stopCommand;
        public ICommand StartCommand => startCommand ??= new DelegateCommand(StartVA);
        public ICommand ResetCommand => resetCommand ??= new DelegateCommand(ResetVA);
        public ICommand StopCommand => stopCommand ??= new DelegateCommand(StopVA);
        public VAViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
        }

        internal void StartVA()
        {
            _mainVM.SetResponseResult("Send Request [Start VA]");
            var req = new RequestControl()
            {
                NodeId = _mainVM.Node.NodeId,
                ChannelIDs = new List<string>() { _mainVM.Channel.ChannelUid },
                Operation = Operations.VA_START
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void ResetVA()
        {
            _mainVM.SetResponseResult("Send Request [Reset VA]");
            var req = new RequestControl()
            {
                NodeId = _mainVM.Node.NodeId,
                ChannelIDs = new List<string>() { _mainVM.Channel.ChannelUid },
                Operation = Operations.VA_RST
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void StopVA()
        {
            _mainVM.SetResponseResult("Send Request [Stop VA]");
            var req = new RequestControl()
            {
                NodeId = _mainVM.Node.NodeId,
                ChannelIDs = new List<string>() { _mainVM.Channel.ChannelUid },
                Operation = Operations.VA_STOP
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }


        public void SetPostURL(IRequest req)
        {
            _mainVM.SetPostURL($"{_mainVM.Node.HostURL}{req.GetResource()}");
        }

        public void SetRequestResult(IRequest req)
        {
            _mainVM.RequestResult = JsonConvert.SerializeObject(req, Formatting.Indented);
        }

        public async Task SetResponseResult(IRequest req)
        {
            ResponseBase? response = await GetResponse(req);

            if (response == null || response.Code != ErrorCode.SUCCESS) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (string.IsNullOrEmpty(_mainVM.Node.NodeId))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_COMPUTING_NODE}\n";
                else if (string.IsNullOrEmpty(_mainVM.Channel.ChannelUid))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_CHANNEL_UID}\n";
                else if (_mainVM.RoI.RoiIDs == null || _mainVM.RoI.RoiIDs.Count == 0)
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_ROI_ID}\n";
                else if (response == null)
                    responseResult = "Error: NO RESPONSE\n";

                var sampleNode = new ResponseControl();
                responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";

                _mainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    _mainVM.SetResponseResult(responseResult);

                    RequestControl request = req as RequestControl;
                    if (request != null && request.Operation == Operations.VA_START)
                        _mainVM.VAStarted?.Invoke();
                    else if (request != null && request.Operation == Operations.VA_STOP)
                        _mainVM.VAStoped?.Invoke();
                }
                else
                    _mainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }

        private void StartMetaService(string hostURL)
        {
            
        }

        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.Node.HostURL));
            return await service.Requset(req) as ResponseControl;

        }
    }
}
