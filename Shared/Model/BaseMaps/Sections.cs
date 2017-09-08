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
                    OSM = new NameValuePair { Name = "CARTO VECTOR", Value = Sources.CartoVector },
					Type = SectionType.Vector,
					Styles = new List<NameValuePair> {
						new NameValuePair { Name = "Voyager", Value = "voyager" },
						new NameValuePair { Name = "Positron", Value = "positron" },
						new NameValuePair { Name = "Darkmatter", Value = "darkmatter" }
					}
				});

				List<NameValuePair> cartoStyles = new List<NameValuePair> {
						new NameValuePair { Name = "Positron", Value = "positron" },
						new NameValuePair { Name = "Dark Matter", Value = "darkmatter" }
				};

				List<NameValuePair> mapzenStyles = new List<NameValuePair> {
					new NameValuePair { Name = "Bright", Value = "styles_mapzen:style" },
					new NameValuePair { Name = "Positron", Value = "styles_mapzen:positron" },
					new NameValuePair { Name = "Dark Matter", Value = "styles_mapzen:positron_dark" }
				};

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "MAPZEN", Value = Sources.Mapzen },
					Type = SectionType.Vector,
					Styles = mapzenStyles
				});

				sections.Add(new Section
				{
					OSM = new NameValuePair { Name = "CARTO RASTER", Value = Sources.CartoRaster },
					Type = SectionType.Raster,
					Styles = cartoStyles
				});

				List<NameValuePair> languageStyles = new List<NameValuePair> {
					new NameValuePair { Name = "Default", Value = "" },
					new NameValuePair { Name = "English", Value = "en" },
					new NameValuePair { Name = "German", Value = "de" },
					new NameValuePair { Name = "Spanish", Value = "es" },
					new NameValuePair { Name = "French", Value = "fr" },
					new NameValuePair { Name = "Russian", Value = "ru" },

					// Italian is supported, but removed in our section for brevity
					//new NameValuePair { Name = "Italian", Value = "it" },

					// Chinese is supported, but disabled in our example, 
					// as it requires an extra asset package (nutibright-v3-full.zip)
					//new NameValuePair { Name = "Chinese", Value = "zh" }
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

		public static string BaseLanguageCode { get { return ""; } }

		public static string BaseStyleValue { get { return Nutiteq.Styles[0].Value; } }
	}
}

