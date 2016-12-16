using System;
using System.Collections.Generic;
using Android.App;
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Named Map", Description = "CARTO data as vector tiles from a named map using VectorListener")]
	public class NamedMapActivity : MapBaseActivity
	{
		List<VectorTileLayer> VectorLayers
		{
			get
			{
				List<VectorTileLayer> layers = new List<VectorTileLayer>();

				for (int i = 0; i < MapView.Layers.Count; i++)
				{
					var layer = MapView.Layers[i];

					if (layer is VectorTileLayer)
					{
						layers.Add(layer as VectorTileLayer);
					}
				}
				return layers;
			}
		}

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView.ConfigureNamedVectorLayers("tpl_69f3eebe_33b6_11e6_8634_0e5db1731f59", delegate
			{
				foreach (VectorTileLayer layer in VectorLayers)
				{
					layer.InitializeVectorTileListener(MapView);
				}
			});

			Projection projection = MapView.Options.BaseProjection;

			// Coordinates are available in the viz.json we download
			MapPos position = projection.FromLatLong(37.32549682016584, -121.94595158100128);
			MapView.FocusPos = position;
			MapView.SetZoom(17, 1);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			foreach (VectorTileLayer layer in VectorLayers)
			{
				layer.VectorTileEventListener = null;
			}
		}
	}
}

