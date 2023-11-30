using System;

namespace NKAPISample.ViewModels
{
    public partial class ScheduleViewModel
    {
        private MainViewModel _mainVM;
        public ScheduleViewModel(MainViewModel mainVM)
        {
            _mainVM = mainVM;
        }

        internal void CreateObject()
        {
            throw new NotImplementedException();
        }

        internal void GetObject()
        {
            throw new NotImplementedException();
        }

        internal void RemoveObject()
        {
            throw new NotImplementedException();
        }
    }
}
