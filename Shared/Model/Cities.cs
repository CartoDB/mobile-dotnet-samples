using System;
using System.Collections.Generic;

namespace Shared.Model
{
    public class Cities
    {
        public static List<City> List
        {
            get
            {
                List<City> list = new List<City>();

                var berlin = new City(
                    "Berlin", 
                    new BoundingBox { MinLon = 13.2285, MaxLon = 13.5046, MinLat = 52.4698, MaxLat = 52.57477 });
                list.Add(berlin);

                var newYork = new City(
                    "New York",
                    new BoundingBox { MinLon = -73.4768, MaxLon = -74.1205, MinLat = 40.4621, MaxLat = 41.0043 });
                list.Add(newYork);

                var madrid = new City(
                    "Madrid", 
                    new BoundingBox { MinLon = -3.7427, MaxLon = -3.6432, MinLat = 40.3825, MaxLat = 40.4904 });
                list.Add(madrid);

                var paris = new City(
                    "Paris", 
                    new BoundingBox { MinLon = 2.1814, MaxLon = 2.4356, MinLat = 48.8089, MaxLat = 48.9176 });
                list.Add(paris);

                var sanFrancisco = new City(
                    "San Francisco", 
                    new BoundingBox { MinLon = -122.54210, MaxLon = -122.3368, MinLat = 37.6622, MaxLat = 37.8173 });
                list.Add(sanFrancisco);

                var london = new City(
                    "London",
                    new BoundingBox { MinLon = -0.5036, MaxLon = 0.3276, MinLat = 51.2871, MaxLat = 51.6939 });
                list.Add(london);

                var mexico = new City(
                    "Mexico City",
                    new BoundingBox { MinLon = -99.329453, MaxLon = -98.937378, MinLat = 19.251515, MaxLat = 19.608956 });
                list.Add(mexico);

                var barcelona = new City(
                    "Barcelona",
                    new BoundingBox { MinLon = 2.0987, MaxLon = 2.2494, MinLat = 41.3456, MaxLat = 41.4540 });
                list.Add(barcelona);

                var tartu = new City(
                    "Tartu",
                    new BoundingBox { MinLon = 26.6548, MaxLon = 26.7901, MinLat = 58.3404, MaxLat = 58.3964 });
                list.Add(tartu);

                var tallinn = new City(
                    "Tallinn",
                    new BoundingBox { MinLon = 24.6165, MaxLon = 24.8740, MinLat = 59.3240, MaxLat = 59.5569 });
                list.Add(tallinn);

                var newDelhi = new City(
                    "New Delhi",
                    new BoundingBox { MinLon = 77.1477, MaxLon = 77.2757, MinLat = 28.5361, MaxLat = 28.6368 });
                list.Add(newDelhi);

                var saoPaolo = new City(
                    "Sao Paolo",
                    new BoundingBox { MinLon = -46.9473, MaxLon = -46.2778, MinLat = -23.7822, MaxLat = -23.4078 });
                list.Add(saoPaolo);

                var rioJanero = new City(
                    "Rio de Janeiro",
                    new BoundingBox { MinLon = -43.4965, MaxLon = -43.1007, MinLat = -22.9621, MaxLat = -22.7210 });
                list.Add(rioJanero);

                return list;
            }
        }
    }

    public class City
    {
        public string Name { get; set; }

        public BoundingBox BoundingBox { get; set; }

        public City(string name, BoundingBox bbox)
        {
            Name = name;
            BoundingBox = bbox;
        }
    }
}
