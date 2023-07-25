namespace PredefineConstant.Enum.Analysis
{
    public enum ClassId
    {
        Person = 0,
        Falldown = 2,
        Violence = 3,
        Head_Head = 4,
        Head_Helmet = 5,
        Person_Cane = 6,
        Person_BathChair = 7,
        Person_Stroller = 8,

        Vehicle_Bike = 100,
        Vehicle = 102,        // car
        Vehicle_Motorcycle = 103,
        Vehicle_Bus = 104,
        Vehicle_Truck = 105,
        Vehicle_Excavator = 106,
        Vehicle_TankTruck = 107,
        Vehicle_Forklift = 108,
        Vehicle_Lemicon = 109,
        Vehicle_Cultivator = 110,
        Vehicle_Tractor = 111,
        Vehicle_ElectricCar = 112,

        Fire_Smoke = 200,
        Fire_Flame = 201,

        Face = 300,

        Bag = 500,
        Cane = 501,
        BathChair = 502,
        Stroller = 503,
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
                    return ObjectType.Person;
                case ClassId.Fire_Smoke:
                case ClassId.Fire_Flame:
                    return ObjectType.Fire;
                case ClassId.Vehicle:
                case ClassId.Vehicle_Bike:
                case ClassId.Vehicle_Motorcycle:
                case ClassId.Vehicle_Bus:
                case ClassId.Vehicle_Truck:
                case ClassId.Vehicle_Excavator:
                case ClassId.Vehicle_TankTruck:
                case ClassId.Vehicle_Forklift:
                case ClassId.Vehicle_Lemicon:
                case ClassId.Vehicle_Cultivator:
                case ClassId.Vehicle_Tractor:
                    return ObjectType.Vehicle;
                case ClassId.Face:
                    return ObjectType.Face;
                case ClassId.Head_Helmet:
                case ClassId.Head_Head:
                    return ObjectType.Head;
                default:
                    return ObjectType.Vehicle;
            }
        }
    }
}
