

using CommunityToolkit.Mvvm.ComponentModel;
using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NKAPISample.ViewModels
{
    
    public partial class DrawingViewModel :ObservableObject
    {
        private MainViewModel _MainVM;
        private ObjectType _SelectedObjectType;

        private object selectedEventType;


        public ObjectType SelectedObjectType
        {
            get => _SelectedObjectType; set
            {
                SetProperty(ref _SelectedObjectType, value);
                SetEventProperty(_SelectedObjectType);
            }
        }        


        public object SelectedEventType { get => selectedEventType; set => SetProperty(ref selectedEventType, value); }
        public ObservableCollection<string> EventList { get; } = new();



        public DrawingViewModel(MainViewModel mainVM)
        {
            _MainVM = mainVM;
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

    }
}
