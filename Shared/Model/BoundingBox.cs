using System;
using System.Globalization;
using Carto.Core;

namespace Shared
{
	public class BoundingBox
	{
		public string Name { get; set; }

		public double MinLat { get; set; }

		public double MinLon { get; set; }

		public double MaxLat { get; set; }

		public double MaxLon { get; set; }

		public MapPos Center { get { return new MapPos((MaxLon + MinLon) / 2, (MaxLat + MinLat) / 2); } }

		public override string ToString()
		{
			return Convert.ToString("bbox(" + MinLon + "," + MinLat + "," + MaxLon + "," + MaxLat + ")", CultureInfo.InvariantCulture);
		}

	}
}
