namespace Engine.Vehicles
{
    class SuperBoots : GroundVehicle
    {
        public override double speed { get; } = 6;
        public override double restInterval { get; } = 60;
        public override string name { get; } = "Super Boots";

        public override double RestDuration(double count) => count == 1 ? 10 : 5;
    }
}