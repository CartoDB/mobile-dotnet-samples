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
					Type = SectionType.Vector,
					Styles = new List<NameValuePair> {
						new NameValuePair { Name = "Bright", Value = "default" },
						new NameValuePair { Name = "Gray", Value = "gray" },
						new NameValuePair { Name = "Dark", Value = "dark" }
					}
				});

				List<NameValuePair> cartoStyles = new List<NameValuePair> {
						new NameValuePair { Name = "Positron", Value = "positron" },
						new NameValuePair { Name = "Dark Matter", Value = "darkmatter" }
				};

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "MapZen", Value = "mapzen.osm" },
					Type = SectionType.Vector,
					Styles = cartoStyles
				});

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "CARTO", Value = "carto.osm" },
					Type = SectionType.Raster,
					Styles = cartoStyles
				});

				List<NameValuePair> languageStyles = new List<NameValuePair> {
					new NameValuePair { Name = "English", Value = "en" },
					new NameValuePair { Name = "German", Value = "de" },
					new NameValuePair { Name = "Spanish", Value = "es" },
					new NameValuePair { Name = "Italian", Value = "it" },
					new NameValuePair { Name = "French", Value = "fr" },
					new NameValuePair { Name = "Russian", Value = "ru" },
					new NameValuePair { Name = "Chinese", Value = "zh" }
				};

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "Language", Value = "lang" },
					Type = SectionType.Language,
					Styles = languageStyles
				});

				return sections;
			}
		}

		public static Section Nutiteq { get { return List[0]; } }

		public static Section Language { get { return List[List.Count - 1]; } }
	}
}

