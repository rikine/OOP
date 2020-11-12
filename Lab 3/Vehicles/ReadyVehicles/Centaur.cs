namespace Engine.Vehicles
{
    class Centaur : GroundVehicle
    {
        public override double speed { get; } = 15;
        public override double restInterval { get; } = 8;
        public override string name { get; } = "Centaur";

        public override double RestDuration(double count) => 2.0;
    }
}