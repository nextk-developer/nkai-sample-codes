namespace PredefineConstant.Enum.Analysis
{
    public enum ClassId
    {
        Unknown = -1,
        Person = 0,
        Falldown = 2,
        Violence = 3,
        Head_Head = 4,
        Head_Helmet = 5,
        Person_Cane = 6,
        Person_BathChair = 7,
        Person_Stroller = 8,

        Vehicle = 100,
        //Vehicle_Micro = 101,
        Vehicle_Bike = 102,
        Vehicle_MotorCycle = 103,
        //Vehicle_Sedan = 104,
        //Vehicle_HatchBack = 105,
        //Vehicle_Universal = 106,
        //Vehicle_LiftBack = 107,
        //Vehicle_Coupe = 108,
        //Vehicle_Cabriolet = 109,
        //Vehicle_Roadster = 110,
        //Vehicle_Targa = 111,
        //Vehicle_Limousine = 112,
        //Vehicle_MuscleCar = 113,
        //Vehicle_SportCart = 114,
        //Vehicle_SuperCar = 115,
        Vehicle_Suv = 116,
        //Vehicle_Crossover = 117,
        Vehicle_Pickup = 118,
        Vehicle_Van = 119,
        //Vehicle_Minivan = 120,
        //Vehicle_Minibus = 121,
        //Vehicle_CamperVan = 122,
        Vehicle_Special = 123,
        //Vehicle_Military = 124,
        Vehicle_Truck = 125,
        Vehicle_FireTruck = 126,
        //Vehicle_ElectricCar = 127,
        //Vehicle_PoliceCar = 128,
        Vehicle_Tractor = 129,
        Vehicle_ForkLift = 130,
        Vehicle_ReadyMixedConcrete = 131,
        Vehicle_Excavators = 132,
        Vehicle_TankLorry = 133,
        Vehicle_Bus = 134,
        Vehicle_Plate = 135,

        Fire_Smoke = 200,
        Fire_Flame = 201,

        Face = 300,

        Bag = 500,
        IndustryCraneHook = 600,
        IndustryCarrier = 601,
        IndustryMaterials = 602,

        //none tracking objects
        NonTrackingObjectFromThis = 1000,
        Glove = 1001,
        FaceShield = 1002,
        SafetyShoes = 1003,
        WeldingSleeve = 1004,
        Cane = 1005,
        BathChair = 1006,
        Stroller = 1007,
        Leak = 1008
    }

    public static class ClassIdEx
    {
        public static ObjectType ToObjectType(this ClassId classid)
        {
            switch (classid)
            {
                case ClassId.Person:
                case ClassId.Falldown:
                case ClassId.Violence:
                case ClassId.Person_Cane:
                case ClassId.Person_BathChair:
                case ClassId.Person_Stroller:
                case ClassId.IndustryCraneHook:
                case ClassId.IndustryCarrier:
                case ClassId.IndustryMaterials:
                    return ObjectType.Person;
                case ClassId.Fire_Smoke:
                case ClassId.Fire_Flame:
                    return ObjectType.Fire;
                case ClassId.Vehicle:
                //case ClassId.Vehicle_Micro:
                case ClassId.Vehicle_Bike:
                case ClassId.Vehicle_MotorCycle:
                //case ClassId.Vehicle_Sedan:
                //case ClassId.Vehicle_HatchBack:
                //case ClassId.Vehicle_Universal:
                //case ClassId.Vehicle_LiftBack:
                //case ClassId.Vehicle_Coupe:
                //case ClassId.Vehicle_Cabriolet:
                //case ClassId.Vehicle_Roadster:
                //case ClassId.Vehicle_Targa:
                //case ClassId.Vehicle_Limousine:
                //case ClassId.Vehicle_MuscleCar:
                //case ClassId.Vehicle_SportCart:
                //case ClassId.Vehicle_SuperCar:
                case ClassId.Vehicle_Suv:
                //case ClassId.Vehicle_Crossover:
                case ClassId.Vehicle_Pickup:
                case ClassId.Vehicle_Van:
                //case ClassId.Vehicle_Minivan:
                //case ClassId.Vehicle_Minibus:
                //case ClassId.Vehicle_CamperVan:
                case ClassId.Vehicle_Special:
                //case ClassId.Vehicle_Military:
                case ClassId.Vehicle_Truck:
                case ClassId.Vehicle_FireTruck:
                //case ClassId.Vehicle_ElectricCar:
                //case ClassId.Vehicle_PoliceCar:
                case ClassId.Vehicle_Tractor:
                case ClassId.Vehicle_ForkLift:
                case ClassId.Vehicle_ReadyMixedConcrete:
                case ClassId.Vehicle_Excavators:
                case ClassId.Vehicle_TankLorry:
                case ClassId.Vehicle_Bus:
                case ClassId.Vehicle_Plate:
                    return ObjectType.Vehicle;
                case ClassId.Face:
                    return ObjectType.Face;
                case ClassId.Head_Helmet:
                case ClassId.Head_Head:
                    return ObjectType.Head;
                default:
                    return ObjectType.Etc;
            }
        }
    }
}
