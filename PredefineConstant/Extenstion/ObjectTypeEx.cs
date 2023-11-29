using PredefineConstant.Enum.Analysis;
using PredefineConstant.Enum.Analysis.EventType;
using System.Collections.Generic;
using System.Linq;

namespace PredefineConstant.Extenstion
{
    public static class ObjectTypeEx
    {
        public static List<ClassId> ToClassIds(this ObjectType ot, IntegrationEventType eventType)
        {
            List<ClassId> classIds = new();
            if (eventType == IntegrationEventType.Falldown)
            {
                // 별도의 모델로 인해 분리
                classIds.Add(ClassId.Falldown);
            }
            else if (eventType == IntegrationEventType.Violence)
            {
                // 별도의 모델로 인해 분리
                classIds.Add(ClassId.Violence);
            }
            else if (eventType == IntegrationEventType.Flame)
            {
                classIds.Add(ClassId.Fire_Flame);
            }
            else if (eventType == IntegrationEventType.Smoke)
            {
                classIds.Add(ClassId.Fire_Smoke);
            }
            else if (eventType == IntegrationEventType.ElderlyPeople)
            {
                classIds.Add(ClassId.Person);
                classIds.Add(ClassId.BathChair);
                classIds.Add(ClassId.Cane);
                classIds.Add(ClassId.Stroller);
                classIds.Add(ClassId.Person_BathChair);
                classIds.Add(ClassId.Person_Cane);
                classIds.Add(ClassId.Person_Stroller);
            }
            else if (eventType == IntegrationEventType.Leak)
            {
                classIds.Add(ClassId.Leak);
            }
            else
            {
                System.Enum.GetValues(typeof(ClassId))
                           .OfType<ClassId>()
                           .Where(x => (int)x < (int)ClassId.NonTrackingObjectFromThis)
                           .Where(x => x.ToString().ToLower().StartsWith(ot.ToString().ToLower()))
                           .ToList()
                           .ForEach(x => classIds.Add(x));
            }

            if (ot == ObjectType.Person)
            {
                classIds.Add(ClassId.FaceShield);
                classIds.Add(ClassId.WeldingSleeve);
                classIds.Add(ClassId.Glove);
                classIds.Add(ClassId.SafetyShoes);
                classIds.Add(ClassId.IndustryCraneHook);
                classIds.Add(ClassId.IndustryCarrier);
                classIds.Add(ClassId.IndustryMaterials);
            }

            return classIds;
        }
    }
}
