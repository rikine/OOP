using System;
using Engine;
using Engine.Races;
using Engine.Vehicles;

namespace OOP_lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var track = new Track(4000);
                var race = new CommonRace(track);
                race.RegisterVehicle(new FlyingCarpet());
                race.RegisterVehicle(new SpeedCamel());
                race.RegisterVehicle(new Mortar());
                race.RegisterVehicle(new Broom());
                race.RegisterVehicle(new Centaur());

                var winner = race.Go();
                Console.WriteLine($"Winner on distance {track.distance} is {winner}");
            }
            catch (WrongDistanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (EmptyRaceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var track = new Track(50);
                var race = new CommonRace(track);
                race.RegisterVehicle(new BactrianCamel());
                race.RegisterVehicle(new Broom());
                race.RegisterVehicle(new FlyingCarpet());
                race.RegisterVehicle(new SuperBoots());
                race.RegisterVehicle(new Centaur());

                var winner = race.Go();
                Console.WriteLine($"Winner on distance {track.distance} is {winner}");
            }
            catch (WrongDistanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (EmptyRaceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                var track = new Track(-100);
                var race = new CommonRace(track);
                race.RegisterVehicle(new BactrianCamel());
                race.RegisterVehicle(new Broom());
                race.RegisterVehicle(new FlyingCarpet());
                race.RegisterVehicle(new SuperBoots());
                race.RegisterVehicle(new Centaur());

                var winner = race.Go();
                Console.WriteLine($"Winner on distance {track.distance} is {winner}");
            }
            catch (WrongDistanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (EmptyRaceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*try
            {
                var track = new Track(-100);
                var race = new AirRace(track);
                race.RegisterVehicle(new BactrianCamel());
                race.RegisterVehicle(new Broom());
                race.RegisterVehicle(new FlyingCarpet());
                race.RegisterVehicle(new SuperBoots());
                race.RegisterVehicle(new Centaur());

                var winner = race.Go();
                Console.WriteLine($"Winner on distance {track.distance} is {winner}");
            }
            catch (WrongDistanceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (EmptyRaceException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }*/
        }
    }
}
