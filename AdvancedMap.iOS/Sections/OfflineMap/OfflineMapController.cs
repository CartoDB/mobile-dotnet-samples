using System;
using Shared.iOS;
using Shared.PackageManagerClient.Mapping;

namespace AdvancedMap.iOS.Sections.OfflineMap
{
    public class OfflineMapController : PackageDownloadBaseController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			ContentView = new OfflineMapView();
            View = ContentView;

			string folder = GetPackageFolder(Mapping.PackageFolder);
			Client = new Mapping(folder);

			SetOnlineMode();
        }

		public override void SetOnlineMode()
		{
			ContentView.SetOnlineMode();
		}

        public override void SetOfflineMode()
		{
			ContentView.SetOfflineMode(Client.Manager);
		}
    }
}
