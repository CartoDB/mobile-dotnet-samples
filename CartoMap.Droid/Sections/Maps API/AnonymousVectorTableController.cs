using System;
using System.Json;
using Android.App;
using Carto.Core;
using Carto.Layers;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Anonymous Vector Table", Description = "Usage of Carto Maps API with vector tiles")]
	public class AnonymousVectorTableController : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleGray);

			JsonValue config = JsonUtils.VectorLayerConfigJson;

			MapView.ConfigureAnonymousVectorLayers(config);

			// Animate map to the content area
			MapPos newYork = MapView.Options.BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}
	}
}

