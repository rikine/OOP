using Engine.Vehicles;

namespace Engine.Races
{
    class AirRace : Race<AirVehicle>
    {
        public AirRace(Track track) : base(track) { }
    }
}