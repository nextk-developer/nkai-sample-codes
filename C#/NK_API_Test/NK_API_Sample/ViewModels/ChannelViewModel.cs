using CommunityToolkit.Mvvm.ComponentModel;
using DevExpress.Mvvm.CodeGenerators;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NKAPISample.ViewModels
{
    public partial class ChannelViewModel : ObservableObject
    {

        private string nodeID;

        public string NodeID { get => nodeID; set => SetProperty(ref nodeID, value); }


        private string channelID;

        public string ChannelID { get => channelID; set => SetProperty(ref channelID, value); }

        private string channelName;

        public string ChannelName { get => channelName; set => SetProperty(ref channelName, value); }

        private string channelDescription;

        public string ChannelDescription { get => channelDescription; set => SetProperty(ref channelDescription, value); }

        private bool isAutoTimeout = false;
        public bool IsAutoTimeout { get => isAutoTimeout; set => SetProperty(ref isAutoTimeout, value); }

        private string channelURI;

        public string ChannelURI { get => channelURI; set => SetProperty(ref channelURI, value); }


        private NKAPIService.API.Channel.Models.InputType channelType;

        public NKAPIService.API.Channel.Models.InputType ChannelType { get => channelType; set => SetProperty(ref channelType, value); }

        private string channelGroupName;

        public ChannelViewModel(MainViewModel mainViewModel)
        {
        }

        public string ChannelGroupName { get => channelGroupName; set => SetProperty(ref channelGroupName, value); }
    }
}
