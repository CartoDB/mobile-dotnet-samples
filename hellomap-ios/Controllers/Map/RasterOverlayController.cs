
using System;

namespace CartoMobileSample
{
	public class RasterOverlayController : MapBaseController
	{
		public override string Name { get { return "Raster Overlay"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use raster layer " +
						 "on top of the vector base map to provide height information.";
			}
		}

	}
}