using System.Collections.Generic;

namespace Engine
{
    abstract class Race<T> where T : IVehicle
    {
        public Track track { get; }
        private List<T> registredVehicles = new List<T>();

        public Race(Track tr) => track = tr;
        public void RegisterVehicle(T vehicle) => registredVehicles.Add(vehicle);

        public IVehicle Go()
        {
            if (registredVehicles.Count == 0)
                throw new EmptyRaceException("No vehicles are registred on a race!!!");

            var minTime = double.MaxValue;
            IVehicle vehicle = null;

            foreach (var veh in registredVehicles)
            {
                var time = veh.Go(track);
                if (time < minTime)
                {
                    minTime = time;
                    vehicle = veh;
                }
            }
            return vehicle;
        }
    }
}