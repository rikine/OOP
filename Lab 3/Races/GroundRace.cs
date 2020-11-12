using Engine.Vehicles;

namespace Engine.Races
{
    class GroundRace : Race<GroundVehicle>
    {
        public GroundRace(Track track) : base(track) { }
    }
}