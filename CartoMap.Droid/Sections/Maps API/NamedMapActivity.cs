using System;
using Android.App;
using Carto.Core;
using Carto.Projections;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityData(Title = "Named Map", Description = "CARTO data as vector tiles from a named map")]
	public class NamedMapActivity : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView.ConfigureNamedVectorLayers("tpl_69f3eebe_33b6_11e6_8634_0e5db1731f59");

			Projection projection = MapView.Options.BaseProjection;

			// Coordinates are available in the viz.json we download
			MapPos position = projection.FromLatLong(37.32549682016584, -121.94595158100128);
			MapView.FocusPos = position;
			MapView.SetZoom(19, 1);
		}
	}
}

