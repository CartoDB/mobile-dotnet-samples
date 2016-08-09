
using System;

namespace CartoMobileSample
{
	public class OfflineRoutingController : MapBaseController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use Carto Mobile SDK routing engine to calculate offline routes. " +
					"First a package is downloaded asynchronously. Once the package is available, routing works offline.";
			}
		}
	}
}

