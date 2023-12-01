using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using NKAPIService.API.VideoAnalysisSetting;
using NKAPIService.API.VideoAnalysisSetting.Models;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Extenstion;
using PredefineConstant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NKAPISample.ViewModels
{
    public partial class ROIViewModel : ObservableObject
    {

        private MainViewModel _mainVM;
        private DelegateCommand createCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;
        private DrawingType drawingType;
        private string _removeTargetID;

        public DrawingType DrawingType { get => drawingType; set => SetProperty(ref drawingType, value); }
        public ICommand CreateCommand => createCommand ??= new DelegateCommand(CreateROI);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(GetROI);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(RemoveROI);

        public ROIViewModel(MainViewModel mainViewModel)
        {
            _mainVM = mainViewModel;
        }


        public void CreateROI()
        {
            _mainVM.SetResponseResult("Send Request [Create ROI]");
            var listRoi = new List<ROIDot>
            {
                new ROIDot { X = 0, Y = 0},
                new ROIDot { X = 1.0, Y = 0},
                new ROIDot { X = 1.0, Y = 1},
                new ROIDot { X = 0, Y = 1},
            };


            var roi = new RequestCreateROI()
            {
                NodeId = _mainVM.NodeID,
                ChannelID = _mainVM.ChannelID,
                EventType = PredefineConstant.Enum.Analysis.EventType.IntegrationEventType.AllDetect,
                RoiDots = listRoi,
                RoiDotsSub = listRoi,
                RoiName = $"ROI",
                RoiFeature = ROIFeature.All,
                RoiNumber = RoiNumber.LANE1,
                RoiType = DrawingType,
                EventFilter = new EventFilter
                {
                    MaxDetectSize = new RoiSize(10, 10),
                    MinDetectSize = new RoiSize(1,1),
                    ObjectsTarget = ObjectType.Person.ToClassIds(IntegrationEventType.AllDetect)
                }
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
        }

        public void GetROI()
        {
            _mainVM.SetResponseResult("Send Request [Get ROI]");
            var roi = new RequestListROI()
            {
                NodeId = _mainVM.NodeID,
                ChannelID=_mainVM.ChannelID
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
        }

        public void RemoveROI()
        {
            _mainVM.SetResponseResult("Send Request [Remove ROI]");
            List<string> ids = new List<string>();

            if (_mainVM.RoiIDs != null && _mainVM.RoiIDs.Count > 0)
            {
                _removeTargetID = _mainVM.RoiIDs[0];
                ids.Add(_mainVM.RoiIDs[0]);
            }

            var roi = new RequestRemoveROI()
            {
                NodeId = _mainVM.NodeID,
                ChannelID = _mainVM.ChannelID,
                ROIIds = ids
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
            _removeTargetID = "";
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

            var response = await GetResponse(req);
            if (response != null && response.Code == ErrorCode.SUCCESS)
            {
                if (response is ResponseCreateROI createRoi)
                {
                    if (_mainVM.RoiIDs == null)
                        _mainVM.RoiIDs = new List<string>();

                    _mainVM.RoiIDs.Add(createRoi.ROIID);
                }
                else if (response is ResponseListROI listRoi)
                {
                    var items = listRoi.RoiItems;
                    if (items != null)
                        _mainVM.RoiIDs = items.Select(x => x.RoiId).ToList();
                }
                else if (response is ResponseBase)
                {
                    if (_mainVM != null)
                        _mainVM.RoiIDs.Remove(_removeTargetID);
                }

                string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                _mainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
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


                if (req is RequestCreateROI)
                {
                    var sampleNode = new ResponseCreateROI();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (req is RequestListROI)
                {
                    var sampleNode = new ResponseListROI();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                else if (req is RequestRemoveROI)
                {
                    var sampleNode = new ResponseBase();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                _mainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }

        }

        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));

            if (req is RequestCreateROI createReq)
                return await service.Requset(createReq) as ResponseCreateROI;
            else if (req is RequestListROI getReq)
                return await service.Requset(getReq) as ResponseListROI;
            else if (req is RequestRemoveROI removeReq) //else if (req is RequestRemoveROI removeReq)
                return await service.Requset(removeReq);

            return null;
        }
    }
}
