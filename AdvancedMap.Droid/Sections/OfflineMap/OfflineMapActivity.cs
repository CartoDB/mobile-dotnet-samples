using System;
using AdvancedMap.Droid.Sections.OfflineMap;
using Android.App;
using Android.OS;
using Shared.PackageManagerClient.Mapping;

namespace AdvancedMap.Droid
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class OfflineMapActivity : PackageDownloadBaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ContentView = new OfflineMapView(this);
            SetContentView(ContentView);
            
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
