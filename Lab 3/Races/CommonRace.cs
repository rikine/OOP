using Engine.Vehicles;

namespace Engine.Races
{
    class CommonRace : Race<IVehicle>
    {
        public CommonRace(Track track) : base(track) { }
    }
}