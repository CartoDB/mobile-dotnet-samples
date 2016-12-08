using System;
using Android.App;
using Carto.Routing;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Online routing", Description = "Online routing with OpenStreetMap data packages")]
	public class OnlineRoutingActivity : BaseRoutingActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Routing.Service = new CartoOnlineRoutingService(Shared.Routing.ServiceSource);
		}

		protected override void SetBaseLayer()
		{
			AddOnlineBaseLayer(Carto.Layers.CartoBaseMapStyle.CartoBasemapStyleDefault);
		}
	}
}
