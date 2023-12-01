using CommunityToolkit.Mvvm.ComponentModel;
using FFmpeg.AutoGen;
using Newtonsoft.Json;
using NKAPIService;
using NKAPIService.API;
using NKAPIService.API.Channel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace NKAPISample.ViewModels
{
    public partial class ScheduleViewModel : ObservableObject
    {
        private MainViewModel _mainVM;
        private DelegateCommand addCommand;
        private DelegateCommand getCommand;
        private DelegateCommand removeCommand;
        private List<DateTime> _selectedDates = new List<DateTime>();

        public ICommand AddCommand => addCommand ??= new DelegateCommand(AddSchedule);
        public ICommand GetCommand => getCommand ??= new DelegateCommand(GetSchedule);
        public ICommand RemoveCommand => removeCommand ??= new DelegateCommand(RemoveSchedule);
        public ScheduleViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
            
        }

        internal void AddSchedule()
        {
            _mainVM.SetResponseResult("Send Request [Add Schedule]");
            List<List<int>> schedule = getDailySchedule();
            var req = new RequestVaSchedule()
            {
                NodeId = _mainVM.NodeID,
                Schedule = schedule,
                ChannelID = _mainVM.ChannelID,
                Except = null
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);
        }

        private void SetRequestResult(IRequest req)
        {
            _mainVM.RequestResult = JsonConvert.SerializeObject(req as RequestVaSchedule, Formatting.Indented);
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


        public async Task<ResponseBase> GetResponse(IRequest schedule)
        {

            APIService service = APIService.Build().SetUrl(new Uri(_mainVM.HostURL));
            if (schedule is RequestVaSchedule req)
                return await service.Requset(req) as ResponseVaSchedule;

            return null;
        }

        private void SetPostURL(RequestVaSchedule req)
        {
            _mainVM.PostURL = $"{_mainVM.HostURL}{req.GetResource()}";
        }

        private List<List<int>> getDailySchedule()
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
            _mainVM.SetResponseResult("Send Request [Get Schedule]");
            var req = new RequestVaSchedule()
            {
                NodeId = _mainVM.NodeID,
                ChannelID = _mainVM.ChannelID,
            };

            SetPostURL(req);
            SetRequestResult(req);
            SetResponseResultAsync(req);

        }

        internal void RemoveSchedule()
        {
            _mainVM.SetResponseResult("Send Request [Remove Schedule]");
        }
    }
}
