using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NKAPISample.ViewModels
{
    public partial class ROIViewModel : ObservableObject
    {
    


        private string channelID;

        public ROIViewModel(MainViewModel mainViewModel)
        {
        }

        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }
    }
}
