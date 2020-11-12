namespace Engine.Vehicles
{
    class Broom : AirVehicle
    {
        public override double speed { get; } = 20;
        public override string name { get; } = "Broom";

        public override double coefDistanceReducer(double distance) => ((int)distance / 1000) / 100;
    }
}