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

			MapView.ConfigureNamedVectorLayers("tpl_0af72420_e533_11e5_9dba_0e5db1731f59");

			Projection projection = MapView.Options.BaseProjection;
			MapPos hiiumaa = projection.FromWgs84(new MapPos(22.7478235498916, 58.8330577553785));

			MapView.FocusPos = hiiumaa;
			MapView.SetZoom(5, 1);
		}
	}
}

