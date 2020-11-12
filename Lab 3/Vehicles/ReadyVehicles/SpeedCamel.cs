namespace Engine.Vehicles
{
    class SpeedCamel : GroundVehicle
    {
        public override double speed { get; } = 40;
        public override double restInterval { get; } = 10;
        public override string name { get; } = "Speed Camel";

        public override double RestDuration(double count) => count == 1 ? 5 : count == 2 ? 6.5 : 8;
    }
}