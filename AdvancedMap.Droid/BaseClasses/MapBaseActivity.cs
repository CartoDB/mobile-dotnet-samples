using System;
using Android.OS;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Projections;
using Carto.Ui;
using Carto.Utils;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class MapBaseActivity : BaseActivity
	{
		protected MapView MapView { get; set; }
		internal Projection BaseProjection { get; set; }
		protected VectorTileLayer BaseLayer { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView = new MapView(this);
			SetContentView(MapView);

			BaseProjection = MapView.Options.BaseProjection;

			Title = GetType().GetTitle();

			if (ActionBar != null)
			{
				ActionBar.Subtitle = GetType().GetDescription();
			}
		}

		protected void AddOnlineBaseLayer(CartoBaseMapStyle style)
		{
			// Initialize map
			BaseLayer = new CartoOnlineVectorTileLayer(style);
			MapView.Layers.Add(BaseLayer);
		}

		protected void AddOfflineBaseLayer(CartoPackageManager manager, CartoBaseMapStyle style)
		{
			var baseLayer = new CartoOfflineVectorTileLayer(manager, style);
			MapView.Layers.Add(baseLayer);
		}
	}
}
