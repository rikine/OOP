namespace Engine.Vehicles
{
    abstract class AirVehicle : IVehicle
    {
        public abstract double speed { get; }
        public abstract string name { get; }
        public abstract double coefDistanceReducer(double distance);

        public double Go(Track track)
        {
            return (track.distance * (1 - coefDistanceReducer(track.distance))) / speed;
        }

        public override string ToString() => name;
    }
}