using CommunityToolkit.Mvvm.ComponentModel;
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
        public ScheduleViewModel(MainViewModel mainVM)
        {
            _MainVM = mainVM;
            
        }

        internal void AddSchedule()
        {
            _MainVM.SetResponseResult("Send Request [Add Schedule]");
            List<List<int>> schedule = GetDailySchedule();
            var req = new RequestVaSchedule()
            {
                NodeId = _MainVM.Node.NodeId,
                Schedule = schedule,
                ChannelID = _MainVM.Channel.ChannelUid,
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
            ResponseBase? response = await GetResponse(req);

            if (response == null || response.Code != ErrorCode.SUCCESS) // 서버 응답 없을 경우 샘플 표출.
            {
                string responseResult = "";
                if (response == null)
                    responseResult = "Error: NO RESPONSE\n";
                else
                    responseResult = $"Error: {response.Code}\n";


                if (req is RequestVaSchedule)
                {
                    var sampleNode = new ResponseVaSchedule();
                    responseResult += $"Response sample:\n{JsonConvert.SerializeObject(sampleNode, Formatting.Indented)}";
                }
                _MainVM.SetResponseResult(responseResult.Replace("null", "\"\""));
            }
            else
            {
                if (response.Code == ErrorCode.SUCCESS)
                {
                    string responseResult = JsonConvert.SerializeObject(response, Formatting.Indented);
                    _MainVM.SetResponseResult(responseResult);
                }
                else
                    _MainVM.SetResponseResult($"[{response.Code}] {response.Message}");
            }
        }


        public async Task<ResponseBase> GetResponse(IRequest schedule)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_MainVM.Node.HostURL));
            if (schedule is RequestVaSchedule req)
                return await service.Requset(req) as ResponseVaSchedule;

            return null;
        }

        private void SetPostURL(RequestVaSchedule req)
        {
            _MainVM.SetPostURL($"{_MainVM.Node.HostURL}{req.GetResource()}");
        }

        private List<List<int>> GetDailySchedule()
        {
            List<List<int>> schedule = new();
            for (int i = 0; i < 7; i++)
            {
                schedule.Add(new List<int> { });
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
                NodeId = _MainVM.Node.NodeId,
                ChannelID = _MainVM.Channel.ChannelUid,
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);

        }

        internal void RemoveSchedule()
        {
            _MainVM.SetResponseResult("Send Request [Remove Schedule]");
            List<List<int>> schedule = new List<List<int>>();
            var req = new RequestVaSchedule()
            {
                NodeId = _MainVM.Node.NodeId,
                Schedule = schedule,
                ChannelID = _MainVM.Channel.ChannelUid,
                Except = null
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);
        }
    }
}
