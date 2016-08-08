
using System;

namespace CartoMobileSample
{
	public class GpsLocationMapController : MapBaseController
	{
		public override string Name { get { return "GPS Location Map"; } }

		public override string Description
		{
			get
			{
				return "Shows user GPS location on map. " +
					"Make sure your app has location permission in Manifest file";
			}
		}

		public GpsLocationMapController()
		{
		}
	}
}

