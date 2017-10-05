using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.IO;

using Carto.Ui;
using Carto.Utils;
using Carto.Layers;

using Carto.Projections;
using Shared.Droid;
using Android.Text;
using Carto.PackageManager;

namespace CartoMap.Droid
{
	public class MapBaseActivity : BaseActivity
	{
		protected MapView MapView { get; set; }
		internal Projection BaseProjection { get; set; }
		protected TileLayer BaseLayer { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView = new MapView(this);
			SetContentView(MapView);

			BaseProjection = MapView.Options.BaseProjection;
		}

		protected void AddOnlineBaseLayer(CartoBaseMapStyle style)
		{
			// Initialize map
			var baseLayer = new CartoOnlineVectorTileLayer(style);
			MapView.Layers.Add(baseLayer);
		}

		protected void AddOfflineBaseLayer(CartoPackageManager manager, CartoBaseMapStyle style)
		{
			// Initialize map
			var baseLayer = new CartoOfflineVectorTileLayer(manager, style);
			MapView.Layers.Add(baseLayer);
		}
	}
}
