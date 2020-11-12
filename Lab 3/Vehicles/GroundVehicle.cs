namespace Engine.Vehicles
{
    abstract class GroundVehicle : IVehicle
    {
        public abstract double speed { get; }
        public abstract string name { get; }
        public abstract double restInterval { get; }
        public abstract double RestDuration(double count);

        public double Go(Track track)
        {
            var totalTime = track.distance / speed;
            for (double i = restInterval, j = 1; i < track.distance / speed; i += restInterval, j++)
            {
                totalTime += RestDuration(j);
            }
            return totalTime;
        }

        public override string ToString() => name;
    }
}