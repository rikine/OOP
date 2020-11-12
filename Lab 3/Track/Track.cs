namespace Engine
{
    internal class Track
    {
        public Track(double dist){
            if (dist <= 0)
                throw new WrongDistanceException($"Distance can't be lower than zero");
            distance = dist;
        }

        public double distance { get; }
    }
}