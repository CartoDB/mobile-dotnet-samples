using CartoMap.WindowsPhone.Pages.Map;
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
                    new MapListItem {
                        Name = "VisJson Map",
                        Description = "High level Carto VisJSON API to display interactive maps",
                        Type = typeof(CartoVisPage)
                    }
                };
            }
        }
    }

    public class MapListItem
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }
    }
}
