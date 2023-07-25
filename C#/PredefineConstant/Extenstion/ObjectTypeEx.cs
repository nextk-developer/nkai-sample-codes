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
                //classIds.Add(ClassId.Person);
                classIds.Add(ClassId.Person_BathChair);
                classIds.Add(ClassId.Person_Cane);
                classIds.Add(ClassId.Person_Stroller);
                //classIds.Add(ClassId.Stroller);
                //classIds.Add(ClassId.Cane);
                //classIds.Add(ClassId.BathChair);
            }
            else
            {
                System.Enum.GetValues(typeof(ClassId))
                           .OfType<ClassId>()
                           .Where(x => x.ToString().ToLower().Contains(ot.ToString().ToLower()))
                           .ToList()
                           .ForEach(x => classIds.Add(x));
            }

            return classIds;
        }
    }
}
