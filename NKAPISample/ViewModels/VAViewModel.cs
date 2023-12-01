﻿using NKAPIService.API.VideoAnalysisSetting.Models;
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
            var req = new RequestControl()
            {
                NodeId = _mainVM.NodeID,
                ChannelIDs = new List<string>() { _mainVM.ChannelID },
                Operation = Operations.VA_START
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void ResetVA()
        {
            var req = new RequestControl()
            {
                NodeId = _mainVM.NodeID,
                ChannelIDs = new List<string>() { _mainVM.ChannelID },
                Operation = Operations.VA_RST
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }

        internal void StopVA()
        {
            var req = new RequestControl()
            {
                NodeId = _mainVM.NodeID,
                ChannelIDs = new List<string>() { _mainVM.ChannelID },
                Operation = Operations.VA_STOP
            };


            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResult(req);
        }


        public void SetPostURL(IRequest req)
        {
            _mainVM.PostURL = $"{_mainVM.HostURL}{req.GetResource()}";
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
                if (string.IsNullOrEmpty(_mainVM.NodeID))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_COMPUTING_NODE}\n";
                else if (string.IsNullOrEmpty(_mainVM.ChannelID))
                    responseResult = $"Error: {ErrorCode.NOT_FOUND_CHANNEL_UID}\n";
                else if (_mainVM.RoiIDs == null || _mainVM.RoiIDs.Count == 0)
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
                }
                else
                    _mainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }


        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));
            return await service.Requset(req) as ResponseControl;

        }
    }
}