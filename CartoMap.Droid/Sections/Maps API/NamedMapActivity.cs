using System;
using Android.App;
using Carto.Core;
using Carto.Projections;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Named Map", Description = "CARTO data as vector tiles from a named map using VectorListener")]
	public class NamedMapActivity : MapBaseActivity
	{
		VectorTileListener listener;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Add base layer so we can attach a vector tile listener to it
			AddOnlineBaseLayer(Carto.Layers.CartoBaseMapStyle.CartoBasemapStyleGray);

			MapView.ConfigureNamedVectorLayers("tpl_69f3eebe_33b6_11e6_8634_0e5db1731f59");

			Projection projection = MapView.Options.BaseProjection;

			// Coordinates are available in the viz.json we download
			MapPos position = projection.FromLatLong(37.32549682016584, -121.94595158100128);
			MapView.FocusPos = position;
			MapView.SetZoom(17, 1);
		}

		protected override void OnResume()
		{
			base.OnResume();

			listener = MapView.InitializeVectorTileListener();
		}

		protected override void OnPause()
		{
			base.OnPause();

			if (listener != null)
			{
				// It'll never be null, if block simply to remove "is never used" warning
				listener = null;
			}
		}
	}
}

