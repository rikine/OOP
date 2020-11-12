namespace Engine.Vehicles
{
    class FlyingCarpet : AirVehicle
    {
        public override double speed { get; } = 10;
        public override string name { get; } = "Flying Carpet";

        public override double coefDistanceReducer(double distance) => distance < 1000 ? 0 : distance < 5000 ? 0.03 : distance < 10000 ? 0.1 : 0.05;
    }
}