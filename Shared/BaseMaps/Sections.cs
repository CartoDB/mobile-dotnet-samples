using System;
using System.Collections.Generic;

namespace Shared
{
	public class Sections
	{
		public static List<Section> List
		{
			get
			{
				List<Section> sections = new List<Section>();

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "Nutiteq", Value = "nutiteq.osm" },
					Type = MapType.Vector,
					Styles = new List<NameValuePair> {
						new NameValuePair { Name = "Bright", Value = "default" },
						new NameValuePair { Name = "Gray", Value = "gray" },
						new NameValuePair { Name = "Dark", Value = "dark" }
					}
				});

				sections.Add(new Section {
					OSM = new NameValuePair { Name = "MapZen", Value = "mapzen.osm" },
					Type = MapType.Vector,
					Styles = new List<NameValuePair> { 
						new NameValuePair { Name = "Positron", Value = "positron" },
						new NameValuePair { Name = "Dark Matter", Value = "dark_matter" }
					}
				});

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "CARTO", Value = "carto.osm" },
					Type = MapType.Raster,
					Styles = new List<NameValuePair> {
						new NameValuePair { Name = "Positron", Value = "positron" },
						new NameValuePair { Name = "Dark Matter", Value = "dark_matter" }
					}
				});

				return sections;
			}
		}
	}
}

