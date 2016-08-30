
using System;
using System.Json;
using Android.App;
using Carto.Core;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "A sample demonstrating how to use Carto Maps API with Raster tiles and UTFGrid")]
	public class CartoUTFGridActivity : VectorBaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			JsonValue config = JsonUtils.UTFGridConfigJson;

			CartoMapUtils.ConfigureUTFGridLayers(MapView, config);

			// Animate map to the content area
			MapPos newYork = MapView.Options.BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}
	}
}

