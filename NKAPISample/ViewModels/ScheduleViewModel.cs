using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class ScheduleViewModel : ObservableObject
    {
        private MainViewModel _MainVM;
        private DelegateCommand _AddCommand;
        private DelegateCommand _GetCommand;
        private DelegateCommand _RemoveCommand;

        public ICommand AddCommand => _AddCommand ??= new DelegateCommand(AddSchedule);
        public ICommand GetCommand => _GetCommand ??= new DelegateCommand(GetSchedule);
        public ICommand RemoveCommand => _RemoveCommand ??= new DelegateCommand(RemoveSchedule);
        public ScheduleViewModel()
        {
            _MainVM = Ioc.Default.GetRequiredService<MainViewModel>();

        }

        internal void AddSchedule()
        {
            _MainVM.SetResponseResult("Send Request [Add Schedule]");
            List<List<int>> schedule = GetDailySchedule();
            var req = new RequestVaSchedule()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                Schedule = schedule,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                Except = null
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);
        }

        private void SetRequestResult(IRequest req)
        {
            _MainVM.RequestResult = JsonConvert.SerializeObject(req as RequestVaSchedule, Formatting.Indented);
        }

        private async Task SetResponseResultAsync(RequestVaSchedule req)
        {
            string responseResult = "";
            // node or channel id null일때 get request 보내면 서버 버그 있어서 아래와 같이 조건문을 통해 예외 처리
            if (_MainVM.CurrentNode == null || string.IsNullOrEmpty(_MainVM.CurrentNode.NodeId))
            {
                responseResult = $"Error: {ErrorCode.NOT_FOUND_COMPUTING_NODE}\nPlease get or create computing node first.\n";
                SetErrorResponseResult(ref responseResult);
            }
            else if (_MainVM.CurrentNode.CurrentChannel == null || string.IsNullOrEmpty(_MainVM.CurrentNode.CurrentChannel.ChannelUid))
            {
                responseResult = $"Error: {ErrorCode.NOT_FOUND_CHANNEL_UID}\nPlease get or create channel first.\n";
                SetErrorResponseResult(ref responseResult);
            }
            else
            {
                ResponseBase? response = await GetResponse(req);
                
                if (response == null || response.Code != ErrorCode.SUCCESS) // 서버 응답 없을 경우 샘플 표출.
                {
                    if (response == null)
                        responseResult = "Error: NO RESPONSE\n";
                    else
                        responseResult = $"Error: {response.Code}\n";

                    SetErrorResponseResult(ref responseResult);
                    
                }
                else
                    responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);                
            }
            _MainVM.SetResponseResult(responseResult);
        }

        private void SetErrorResponseResult(ref string responseResult)
        {
            var sampleNode = new ResponseVaSchedule();
            responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
        }

        public async Task<ResponseBase> GetResponse(IRequest schedule)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.CurrentNode.HostURL));
            if (schedule is RequestVaSchedule req)
                return await service.Requset(req) as ResponseVaSchedule;

            return null;
        }

        private void SetPostURL(RequestVaSchedule req)
        {
            _MainVM.SetPostURL($"{_MainVM.CurrentNode.HostURL}{req.GetResource()}");
        }

        private List<List<int>> GetDailySchedule(bool isRemove = false)
        {
            List<List<int>> schedule = new();
            for (int i = 0; i < 7; i++)
            {
                schedule.Add(new List<int> { });
                if (isRemove)
                    continue;

                for (int j = 0; j < 24; j++)
                {
                    schedule[i].Add(j);
                }
            }

            return schedule;
        }

        internal void GetSchedule()
        {
            _MainVM.SetResponseResult("Send Request [Get Schedule]");
            var req = new RequestVaSchedule()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);

        }

        internal void RemoveSchedule()
        {
            _MainVM.SetResponseResult("Send Request [Remove Schedule]");
            List<List<int>> schedule = GetDailySchedule(true);
            var req = new RequestVaSchedule()
            {
                NodeId = _MainVM.CurrentNode.NodeId,
                Schedule = schedule,
                ChannelID = _MainVM.CurrentNode.CurrentChannel.ChannelUid,
                Except = new()
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);
        }
    }
}
