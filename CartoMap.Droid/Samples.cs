using System;
using System.Collections.Generic;

namespace CartoMap.Droid
{
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		// List of demo activities
		public static List<Type> List = new List<Type>(new Type[] {

			typeof(CartoJSHeader),
			typeof(CountriesVisActivity),
			typeof(DotsVisActivity),
			typeof(FontsVisActivity),

			// TODO: Currently disabled, as the feature is a work in progress
			//typeof(ImportHeader),
			//typeof(TilePackagerActivity),

			typeof(MapsHeader),
			typeof(AnonymousRasterTableActivity),
			typeof(AnonymousVectorTableActivity),
			typeof(NamedMapActivity),

			typeof(SQLHeader),
			typeof(SQLServiceActivity),
				
			typeof(TorqueHeader),
			typeof(TorqueShipsActivity),
		});

		public static Type FromPosition(int position)
		{
			return List[position];
		}
	}
}

