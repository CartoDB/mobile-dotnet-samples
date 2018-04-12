
using Carto.Routing;
using Shared;

namespace AdvancedMap.iOS
{
	public class OfflineRoutingController : BaseRoutingController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description { get { return "Offline routing with OpenStreetMap data packages"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            string text = "Long click on the map to set route start point";
            ContentView.Banner.Show(text);

			ContentView.SetOnlineMode();
            SetOnlineMode();
		}

        public override void SetOnlineMode()
		{
            Routing.Service = new CartoOnlineRoutingService(Sources.NutiteqRouting);
            Routing.BringLayersToFront();
		}

		public override void SetOfflineMode()
		{
			string text = "Click the globe icon to download routing packages";
			ContentView.Banner.Show(text);
			Routing.Service = new PackageManagerValhallaRoutingService(Routing.Manager);
            Routing.BringLayersToFront();
		}
	}
}
