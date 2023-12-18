using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.VideoAnalysisSetting;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using PredefineConstant.Extenstion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vortice.MediaFoundation;
using ObjectType = PredefineConstant.Enum.Analysis.ObjectType;

namespace NKAPISample.ViewModels
{
    public partial class ROIViewModel : ObservableObject
    {

        private MainViewModel _MainVM;
        private RelayCommand _CreateCommand;
        private RelayCommand _GetCommand;
        private RelayCommand _RemoveCommand;
        private DrawingType _DrawingType;
        private string _RemoveTargetID;
        private ObjectType _SelectedObjectType;
        private object _SelectedEventType;
        

        private bool _IsLineEnabled;
        public bool IsLineEnabled { get => _IsLineEnabled; private set => SetProperty(ref _IsLineEnabled, value); }

        private bool _IsMultiLineEnabled;
        public bool IsMultiLineEnabled { get => _IsMultiLineEnabled; private set => SetProperty(ref _IsMultiLineEnabled, value); }

        private bool _IsDrawingEnabled;
        public bool IsDrawingEnabled { get => _IsDrawingEnabled; private set => SetProperty(ref _IsDrawingEnabled, value); }
        private bool _IsMultiPolygonEnabled;
        public bool IsMultiPolygonEnabled { get => _IsMultiPolygonEnabled; private set => SetProperty(ref _IsMultiPolygonEnabled, value); }

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
        public IRelayCommand CreateCommand => _CreateCommand ??= new RelayCommand(CreateROI);
        public IRelayCommand GetCommand => _GetCommand ??= new RelayCommand(GetROI);
        public IRelayCommand RemoveCommand => _RemoveCommand ??= new RelayCommand(RemoveROI);

        public List<RoiPoint> CurrentRangeList { get; private set; } = new();
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
                case IntegrationEventType.FloodedOrSnowRoad:
                    drawingType = DrawingType.MultiPolygon;
                    break;

                //line
                case IntegrationEventType.LineCrossing:
                case IntegrationEventType.LineEnter:
                case IntegrationEventType.LineStraight:
                case IntegrationEventType.LineRightTurn:
                case IntegrationEventType.LineLeftTurn:
                case IntegrationEventType.LineUTurn:
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

            DrawingType = drawingType;
            IsLineEnabled = _DrawingType == DrawingType.Line;
            IsMultiLineEnabled = _DrawingType == DrawingType.MultiLine;
            IsDrawingEnabled = _DrawingType == DrawingType.Rect;
            IsMultiPolygonEnabled = _DrawingType == DrawingType.MultiPolygon;

            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.ClearRange();
        }


        public void CreateROI()
        {
            _MainVM.SetResponseResult("Send Request [Create ROI]");
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            CurrentRangeList = videoVM.GetRange();
            if (!CurrentRangeList.Any())
            {
                CurrentRangeList = new()
                {
                    new(){
                        RoiNumber=0,
                        RoiType = DrawingType.All,
                        Points = new List<ROIDot>()
                        {
                            new ROIDot { X = 0, Y = 0},
                            new ROIDot { X = 1.0, Y = 0},
                            new ROIDot { X = 1.0, Y = 1},
                            new ROIDot { X = 0, Y = 1},
                        }
                    }
                };
            }

            IntegrationEventType eventTypeResult = (IntegrationEventType)Enum.Parse(typeof(IntegrationEventType), _SelectedEventType.ToString());
            Dictionary<string, string> param = getParam(DrawingType, eventTypeResult);
            var roi = new RequestAddOrUpdateRoi()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                RoiId = string.Empty,
                EventType = eventTypeResult,
                RoiType = DrawingType,
                MinMaxSize = new MinMaxSize() { MinDetectSize = new(0, 0), MaxDetectSize = new(1, 1) },
                ObjectsFilter = _SelectedObjectType.ToClassIds(eventTypeResult),
                ObjectType = _SelectedObjectType,
                Params = param,
                RoiName = string.Empty,
                RoiPoints = CurrentRangeList

            };

            SetPostURL(roi);
            SetRequestResult(roi);
            SetResponseResult(roi);
        }

        private Dictionary<string, string> getParam(DrawingType drawingType, IntegrationEventType eventType)
        {
            double stayTime = 0;
            InsideRoiType insideRoiType = InsideRoiType.CenterX_BottomY;
            List<double> customPoints = new();
            double intersection = 0;
            string thumbnailRatio = "1, 1";
            string threshold = string.Join(",", SelectedObjectType.ToClassIds(eventType).Select(c => $"{(int)c},0.5"));
            string directional = "0, 0";
            double maxExitTime = 2;

            return new()
            {
                { ParamKeys.StayTimeSec, stayTime.ToString() },
                { ParamKeys.InsideRoiType, ((int)insideRoiType).ToString() },
                { ParamKeys.CustomReferencePoint, string.Join(",", customPoints) },
                { ParamKeys.Intersection, intersection.ToString() },
                { ParamKeys.ThumbnailRatio, thumbnailRatio },
                { ParamKeys.Threshold, threshold },
                { ParamKeys.Directional, directional },
                { ParamKeys.MaxExitTime, maxExitTime.ToString() }
            };
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
            _RemoveTargetID = _MainVM.GetCurrentRoiID();
        }

        public void SetPostURL(IRequest req)
        {
            _MainVM.SetPostURL($"{_MainVM.CurrentNode.HostURL}{req.GetResource()}");
        }

        public void SetRequestResult(IRequest req)
        {
            if (req is RequestAddOrUpdateRoi createReq)
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
                if (response is ResponseAddOrUpdateRoi createRoi)
                {
                    _MainVM.AddRoi(createRoi.Roi);

                }
                else if (response is ResponseListROI listRoi)
                {
                    if (listRoi != null && listRoi.RoiItems.Any())
                    {
                        _Items = listRoi.RoiItems;
                        _MainVM.SetRoi(listRoi.RoiItems);
                    }
                    else
                    {
                        _Items.Clear();
                        var sampleNode = new ResponseListROI();
                        _MainVM.SetResponseResult($"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}");
                    }
                }
                else if (response is ResponseBase)
                {
                    if (_Items != null && _Items.Any())
                    {
                        _Items.Remove(_Items.Last());
                        _MainVM.SetRoi(_Items);
                    }
                    else
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


                if (req is RequestAddOrUpdateRoi)
                {
                    var sampleNode = new RequestAddOrUpdateRoi();
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

            if (req is RequestAddOrUpdateRoi createReq)
                return await service.Requset(createReq) as ResponseAddOrUpdateRoi;
            else if (req is RequestListROI getReq)
                return await service.Requset(getReq) as ResponseListROI;
            else if (req is RequestRemoveROI removeReq) //else if (req is RequestRemoveROI removeReq)
                return await service.Requset(removeReq);

            return null;
        }

        private RelayCommand allRangeCommand;
        public IRelayCommand AllRangeCommand => allRangeCommand ??= new RelayCommand(AllRange);

        private void AllRange()
        {
            // video view에 전체 영역 자동 컬러링했다가 3초후 꺼지기!
            CurrentRangeList.Clear();
            DrawingType = DrawingType.All;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.All);
        }

        private RelayCommand selectRangeCommand;
        public IRelayCommand SelectRangeCommand => selectRangeCommand ??= new RelayCommand(DrawRectangle);

        private void DrawRectangle()
        {
            // video view에서 사각형 영역 선택할 수 있게 mouse down, mouse up 이벤트 받아서 처리하기.
            CurrentRangeList.Clear();
            DrawingType = DrawingType.Rect;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.Rect);
        }

        private RelayCommand polygonCommand;
        public IRelayCommand PolygonCommand => polygonCommand ??= new RelayCommand(Polygon);

        private void Polygon()
        {
            // video view에서 mouse up 이벤트로 좌표 받아서 좌표 리스트 폴리곤 만들기
            CurrentRangeList.Clear();
            DrawingType = DrawingType.Polygon;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.Polygon);
        }


        private RelayCommand lineCommand;
        public IRelayCommand LineCommand => lineCommand ??= new RelayCommand(DrawSingleLine);

        

        private void DrawSingleLine()
        {
            CurrentRangeList.Clear();
            DrawingType = DrawingType.Line;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.Line);
        }

        private RelayCommand multiLineCommand;
        private List<RoiModel> _Items = new();

        public IRelayCommand MultiLineCommand => multiLineCommand ??= new RelayCommand(DrawMultiLine);

        private void DrawMultiLine()
        {
            CurrentRangeList.Clear();
            DrawingType = DrawingType.MultiLine;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.MultiLine);
        }

        private RelayCommand multiPolygonCommand;
        public ICommand MultiPolygonCommand => multiPolygonCommand ??= new RelayCommand(MultiPolygon);

        private void MultiPolygon()
        {
            CurrentRangeList.Clear();
            DrawingType = DrawingType.MultiPolygon;
            var videoVM = Ioc.Default.GetService<VideoViewModel>();
            videoVM.SetDrawingMode(DrawingType.MultiPolygon);
        }
    }
}
