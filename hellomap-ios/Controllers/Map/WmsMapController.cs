
using System;

namespace CartoMobileSample
{
	public class WmsMapController : MapBaseController
	{
		public override string Name { get { return ""; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use WMS service raster on top of the vector base map";
			}
		}

	}
}

