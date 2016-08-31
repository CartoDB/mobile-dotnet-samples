using System;
using System.Collections.Generic;

namespace CartoMap.Droid
{
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		// List of demo activities
		public static List<Type> List = new List<Type>(new Type[] {
			typeof (CartoRasterTileActivity),
			typeof (CartoSQLActivity),
			typeof (CartoTorqueActivity),
			typeof (CartoUTFGridActivity),
			typeof (CartoVisJsonActivity)
		});

		public static Type FromPosition(int position)
		{
			return List[position];
		}
	}
}

