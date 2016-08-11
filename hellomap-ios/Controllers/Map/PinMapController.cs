
using System;

namespace CartoMobileSample
{
	public class PinMapController : VectorMapBaseController
	{
		public override string Name { get { return "Pin Map"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use markers on the map: " +
						 "creating a data source, creating a layer, loading marker bitmaps, creating a style " +
						 "and adding the marker to the data source.";
			}
		}

	}
}

