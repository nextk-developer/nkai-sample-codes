using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using NKAPISample;
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
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using Vortice.MediaFoundation;
using ObjectType = PredefineConstant.Enum.Analysis.ObjectType;

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
        private ObjectType _SelectedObjectType;
        private object _SelectedEventType;
        private DrawingType _CurrentDrawingType;
        public DrawingType CurrentDrawingType { get => _CurrentDrawingType; private set => SetProperty(ref _CurrentDrawingType, value); }

        private bool _IsLineEnabled;
        public bool IsLineEnabled { get => _IsLineEnabled; private set => SetProperty(ref _IsLineEnabled, value); }

        private bool _IsMultiLineEnabled;
        public bool IsMultiLineEnabled { get => _IsMultiLineEnabled; private set => SetProperty(ref _IsMultiLineEnabled, value); }

        private bool _IsDrawingEnabled;
        public bool IsDrawingEnabled { get => _IsDrawingEnabled; private set => SetProperty(ref _IsDrawingEnabled, value); }

        public ObjectType SelectedObjectType
        {
            get => _SelectedObjectType; 
            set
            {
                SetProperty(ref _SelectedObjectType, value);
                SetEventProperty(_SelectedObjectType);
            }
        }

        public object SelectedEventType { 
            get => _SelectedEventType; 
            set
            {
                SetProperty(ref _SelectedEventType, value);
                SetEnableDrawingType(_SelectedEventType);
            }
        }

        public ObservableCollection<string> EventList { get; } = new();

        public DrawingType DrawingType { get => _DrawingType; set => SetProperty(ref _DrawingType, value); }
        public ICommand CreateCommand => _CreateCommand ??= new DelegateCommand(CreateROI);
        public ICommand GetCommand => _GetCommand ??= new DelegateCommand(GetROI);
        public ICommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(RemoveROI);


        public ROIViewModel()
        {
            _MainVM = Ioc.Default.GetService<MainViewModel>();
            SetEventProperty(ObjectType.Person);
        }


        private void SetEventProperty(ObjectType selectedObjectType)
        {

            EventList.Clear();
            switch (selectedObjectType)
            {
                case ObjectType.Person:
                    var personTypes = Enum.GetNames<PersonEventType>().Where(type => !type.Equals(PersonEventType.Unknown.ToString())).ToList();
                    personTypes.ForEach(type => EventList.Add(type));
                    break;

                case ObjectType.Vehicle:
                    var vehicleTypes = Enum.GetNames<VehicleEventType>().Where(type => !type.Equals(VehicleEventType.Unknown.ToString())).ToList();
                    vehicleTypes.ForEach(type => EventList.Add(type));
                    break;

                case ObjectType.Face:
                    var faceTypes = Enum.GetNames<FaceEventType>().Where(type => !type.Equals(FaceEventType.Unknown.ToString())).ToList();
                    faceTypes.ForEach(type => EventList.Add(type));
                    break;

                case ObjectType.Fire:
                    var fireTypes = Enum.GetNames<FireEventType>().Where(type => !type.Equals(FireEventType.Unknown.ToString())).ToList();
                    fireTypes.ForEach(type => EventList.Add(type));
                    break;

                case ObjectType.Head:
                    var headTypes = Enum.GetNames<HeadEventType>().Where(type => !type.Equals(HeadEventType.Unknown.ToString())).ToList();
                    headTypes.ForEach(type => EventList.Add(type));
                    break;

                case ObjectType.Etc:
                    var etcTypes = Enum.GetNames<EtcEventType>().Where(type => !type.Equals(EtcEventType.Unknown.ToString())).ToList();
                    etcTypes.ForEach(type => EventList.Add(type));
                    break;


                default:
                    break;
            }

            SelectedEventType = EventList[0];
        }


        private void SetEnableDrawingType(object selectedEventType)
        {
            if (selectedEventType == null)
                return;

            IntegrationEventType eventTypeResult = (IntegrationEventType)Enum.Parse(typeof(IntegrationEventType), selectedEventType.ToString());
            DrawingType drawingType;
            switch (eventTypeResult)
            {
                //line
                case IntegrationEventType.LineCrossing:
                case IntegrationEventType.LineEnter:
                    drawingType = DrawingType.Line;
                    break;
                //multiline
                case IntegrationEventType.Direction:
                case IntegrationEventType.Straight:
                case IntegrationEventType.LeftTurn:
                case IntegrationEventType.RightTurn:
                case IntegrationEventType.UTurn:
                case IntegrationEventType.IntervalVelocity:
                    drawingType = DrawingType.MultiLine;
                    break;
                default:
                    drawingType = DrawingType.Rect;
                    break;
            }

            CurrentDrawingType = drawingType;
            IsLineEnabled = _CurrentDrawingType == DrawingType.Line;
            IsMultiLineEnabled = _CurrentDrawingType == DrawingType.MultiLine;
            IsDrawingEnabled = _CurrentDrawingType == DrawingType.Rect;
            _MainVM.ROIEventTypeSelected?.Invoke(drawingType);
        }


        public void CreateROI()
        {
            _MainVM.SetResponseResult("Send Request [Create ROI]");
            if (CurrentRangeList == null)
            {
                CurrentRangeList = new List<ROIDot>
                {
                    new ROIDot { X = 0, Y = 0},
                    new ROIDot { X = 1.0, Y = 0},
                    new ROIDot { X = 1.0, Y = 1},
                    new ROIDot { X = 0, Y = 1},
                };
            }

            IntegrationEventType eventTypeResult = (IntegrationEventType)Enum.Parse(typeof(IntegrationEventType), _SelectedEventType.ToString());

            var roi = new RequestCreateROI()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                EventType = eventTypeResult,
                RoiDots = CurrentRangeList,
                RoiDotsSub = CurrentRangeList,
                RoiName = $"ROI",
                RoiFeature = ROIFeature.All,
                RoiNumber = RoiNumber.LANE1,
                RoiType = DrawingType,
                EventFilter = new EventFilter
                {
                    MaxDetectSize = new RoiSize(1, 1),
                    MinDetectSize = new RoiSize(0, 0),
                    ObjectsTarget = _SelectedObjectType.ToClassIds(eventTypeResult)
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

        private DelegateCommand allRangeCommand;
        public ICommand AllRangeCommand => allRangeCommand ??= new DelegateCommand(AllRange);

        private void AllRange()
        {
            // video view에 전체 영역 자동 컬러링했다가 3초후 꺼지기!
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            CurrentRangeList = videoVM.GetRange(DrawingType.All);
        }

        private DelegateCommand selectRangeCommand;
        public ICommand SelectRangeCommand => selectRangeCommand ??= new DelegateCommand(SelectRange);

        private void SelectRange()
        {
            // video view에서 사각형 영역 선택할 수 있게 mouse down, mouse up 이벤트 받아서 처리하기.
        }

        private DelegateCommand polygonCommand;
        public ICommand PolygonCommand => polygonCommand ??= new DelegateCommand(Polygon);

        private void Polygon()
        {
            // video view에서 mouse up 이벤트로 좌표 받아서 좌표 리스트 폴리곤 만들기
        }

        private DelegateCommand multiLineCommand;
        public ICommand MultiLineCommand => multiLineCommand ??= new DelegateCommand(MultiLine);

        

        private void MultiLine()
        {
            // video view에서 mouse up 이벤트로 두 점씩 여러개 좌표 받기
        }

        private DelegateCommand lineCommand;
        public ICommand LineCommand => lineCommand ??= new DelegateCommand(Line1);

        public List<ROIDot> CurrentRangeList { get; private set; }

        private void Line1()
        {
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            var range = videoVM.GetRange(DrawingType.Line);
        }
    }
}
