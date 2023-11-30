using System;

namespace NKAPISample.ViewModels
{
    public partial class VAViewModel
    {
        private MainViewModel _mainVM;

        public VAViewModel(MainViewModel mainVM)
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
