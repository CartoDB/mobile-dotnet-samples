using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartoMap.WindowsPhone
{
    public class Samples
    {
        public static object List {
            get {
                return new[] {
                    new Map { Name = "VisJson Map", Description = "High level Carto VisJSON API to display interactive maps" }
                };
            }
        }
    }

    public class Map
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
