using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System;


namespace PredefineConstant.Extenstion
{
    public static class IntegrationEventTypeEx
    {
        public static object ToParse(this IntegrationEventType eventType, ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Person:
                    return (PersonEventType)System.Enum.Parse(typeof(PersonEventType), eventType.ToString());
                case ObjectType.Vehicle:
                    return (VehicleEventType)System.Enum.Parse(typeof(VehicleEventType), eventType.ToString());
                case ObjectType.Face:
                    return (FaceEventType)System.Enum.Parse(typeof(FaceEventType), eventType.ToString());
                case ObjectType.Fire:
                    return (FireEventType)System.Enum.Parse(typeof(FireEventType), eventType.ToString());
                case ObjectType.Head:
                    return (HeadEventType)System.Enum.Parse(typeof(HeadEventType), eventType.ToString());
                default:
                    break;
            }

            throw new NotImplementedException();
        }
    }
}
