namespace Engine.Vehicles
{
    class Mortar : AirVehicle
    {
        public override double speed { get; } = 8;
        public override string name { get; } = "Mortar";

        public override double coefDistanceReducer(double distance) => 0.06;
    }
}