using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
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

        private MainViewModel _MainVM;
        private DelegateCommand _CreateCommand;
        private DelegateCommand _GetCommand;
        private DelegateCommand _RemoveCommand;
        private DrawingType _DrawingType;
        private string _RemoveTargetID;

        

        public DrawingType DrawingType { get => _DrawingType; set => SetProperty(ref _DrawingType, value); }
        public ICommand CreateCommand => _CreateCommand ??= new DelegateCommand(CreateROI);
        public ICommand GetCommand => _GetCommand ??= new DelegateCommand(GetROI);
        public ICommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(RemoveROI);


        public ROIViewModel()
        {
            _MainVM = Ioc.Default.GetService<MainViewModel>();
        }


        public void CreateROI()
        {
            _MainVM.SetResponseResult("Send Request [Create ROI]");
            var listRoi = new List<ROIDot>
            {
                new ROIDot { X = 0, Y = 0},
                new ROIDot { X = 1.0, Y = 0},
                new ROIDot { X = 1.0, Y = 1},
                new ROIDot { X = 0, Y = 1},
            };


            var roi = new RequestCreateROI()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                EventType = PredefineConstant.Enum.Analysis.EventType.IntegrationEventType.AllDetect,
                RoiDots = listRoi,
                RoiDotsSub = listRoi,
                RoiName = $"ROI",
                RoiFeature = ROIFeature.All,
                RoiNumber = RoiNumber.LANE1,
                RoiType = DrawingType,
                EventFilter = new EventFilter
                {
                    MaxDetectSize = new RoiSize(1, 1),
                    MinDetectSize = new RoiSize(0, 0),
                    ObjectsTarget = ObjectType.Person.ToClassIds(IntegrationEventType.AllDetect)
                }
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
        }

        public void GetROI()
        {
            _MainVM.SetResponseResult("Send Request [Get ROI]");
            var roi = new RequestListROI()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID=_MainVM.CurrentNode.CurrentChannel.ChannelUid
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
        }

        public void RemoveROI()
        {
            _MainVM.SetResponseResult("Send Request [Remove ROI]");
            List<string> ids = new List<string>();

            _RemoveTargetID = _MainVM.GetCurrentRoiID();
            ids.Add(_RemoveTargetID);


            var roi = new RequestRemoveROI()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                ROIIds = ids
            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
            _RemoveTargetID = "";
        }

        public void SetPostURL(IRequest req)
        {
            _MainVM.SetPostURL($"{_MainVM.CurrentNode.HostURL}{req.GetResource()}");
        }

        public void SetRequestResult(IRequest req)
        {
            if (req is RequestCreateROI createReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(createReq, Formatting.Indented);
            else if (req is RequestListROI getReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(getReq, Formatting.Indented);
            else if (req is RequestRemoveROI removeReq)
                _MainVM.RequestResult = JsonConvert.SerializeObject(removeReq, Formatting.Indented);
        }

        public async Task SetResponseResult(IRequest req)
        {

            var response = await GetResponse(req);
            if (response != null && response.Code == ErrorCode.SUCCESS)
            {
                if (response is ResponseCreateROI createRoi)
                {
                    var requestCreateROI = req as RequestCreateROI;
                    _MainVM.AddRoi(requestCreateROI.NodeId, requestCreateROI.ChannelID, createRoi.ROIID, requestCreateROI.EventType, ObjectType.Person, requestCreateROI.RoiDots, requestCreateROI.RoiDotsSub,
                        requestCreateROI.RoiName, requestCreateROI.RoiFeature, requestCreateROI.RoiNumber, requestCreateROI.RoiType, requestCreateROI.EventFilter);
                }
                else if (response is ResponseListROI listRoi)
                {
                    var items = listRoi.RoiItems;
                    if (items != null)
                    {
                        var roi = listRoi.RoiItems.LastOrDefault();
                        if (roi != null)
                            _MainVM.SetCurrentRoi(roi);
                    }
                }
                else if (response is ResponseBase)
                {
                    if (_MainVM != null)
                        _MainVM.RemoveRoi();
                }

                string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
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
                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }

        }

        public async Task<ResponseBase> GetResponse(IRequest req)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.CurrentNode.HostURL));

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
