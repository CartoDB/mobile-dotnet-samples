using System;
using Carto.Routing;
using Shared;

namespace AdvancedMap.iOS
{
	public class OnlineRoutingController : BaseRoutingController
	{
		public override string Name { get { return "Online Routing"; } }

		public override string Description { get { return "Online routing with OpenStreetMap data packages"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            Routing.Service = new CartoOnlineRoutingService(Sources.OnlineRouting + Sources.TransportMode_Car);
		}

		protected override void SetBaseLayer()
		{
			AddOnlineBaseLayer(Carto.Layers.CartoBaseMapStyle.CartoBasemapStyleDefault);
		}
	}
}
