namespace Engine.Vehicles
{
    class BactrianCamel : GroundVehicle
    {
        public override double speed { get; } = 10;
        public override double restInterval { get; } = 30;
        public override string name { get; } = "BactrianCamel";

        public override double RestDuration(double count) => count == 1 ? 5.0 : 8.0;
    }
}