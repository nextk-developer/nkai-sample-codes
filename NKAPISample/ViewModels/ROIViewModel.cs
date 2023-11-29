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
    public partial class ROIViewModel : ObservableObject, ICommnunication
    {


        private string postURI;
        private string requestResult;
        private string responseResult;
        private string nodeID;
        private string channelID;
        private string hostURL;

        public string PostURL { get => postURI; set => SetProperty(ref postURI, value); }
        public string RequestResult { get => requestResult; set => SetProperty(ref requestResult, value); }
        public string ResponseResult { get => responseResult; set => SetProperty(ref responseResult, value); }
        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }
        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
        public string HostURL { get => hostURL; set => SetProperty(ref hostURL, value); }

        public ROIViewModel(MainViewModel mainViewModel)
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
            PostURL = $"{HostURL}{req.GetResource()}";
        }

        public void SetRequestResult(IRequest req)
        {
            if (req is RequestCreateROI createReq)
                RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (req is RequestListROI getReq)
                RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (req is RequestRemoveROI removeReq)
                RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
        }

        public async Task SetResponseResult(IRequest req)
        {
            ResponseBase? response = await GetResponse(req);
            if (response != null)
            {
                
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    ResponseResult = responseResult;
                }
            }
        }

        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(HostURL));

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
