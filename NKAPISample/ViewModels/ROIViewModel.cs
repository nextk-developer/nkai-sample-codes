using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using FFmpeg.AutoGen;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using NKAPIService.API.ComputingNode;
using NKAPIService.API.VideoAnalysisSetting;
using PredefineConstant.Enum.Camera;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NKAPISample.ViewModels
{
    public partial class ROIViewModel : ObservableObject
    {

        private MainViewModel _mainVM;

        public string NodeID { get => _mainVM.NodeID; set => _mainVM.NodeID = value; }
        public string ChannelID { get => _mainVM.ChannelID; set => _mainVM.ChannelID = value; }
        

        public ROIViewModel(MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
        }

        public void CreateObject()
        {
            var roi = new RequestCreateROI()
            {
               
            };
        }

        public void GetObject()
        {
            var roi = new RequestListROI()
            {

            };
        }

        public void RemoveObject()
        {
            var roi = new RequestRemoveROI()
            {

            };
        }

        public void SetPostURL(IRequest req)
        {
            _mainVM.PostURL = $"{_mainVM.HostURL}{req.GetResource()}";
        }

        public void SetRequestResult(IRequest req)
        {
            if (req is RequestCreateROI createReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (req is RequestListROI getReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (req is RequestRemoveROI removeReq)
                _mainVM.RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
        }

        public async Task SetResponseResult(IRequest req)
        {
            ResponseBase? response = await GetResponse(req);
            if (response != null)
            {
                
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    _mainVM.ResponseResult = responseResult;
                }
            }
        }

        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));

            if (req is RequestCreateROI createReq)
                return await service.Requset(createReq) as ResponseCreateROI;
            else if (req is RequestListROI getReq)
                return await service.Requset(getReq) as ResponseListROI;
            else if (req is RequestControl removeReq) //else if (req is RequestRemoveROI removeReq)
                return await service.Requset(removeReq) as ResponseControl;

            return null;
        }
    }
}
