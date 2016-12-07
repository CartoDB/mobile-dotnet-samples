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

namespace Shared.Droid
{
	public class MapBaseActivity : BaseActivity
	{
		public static int ViewResource { get; set; }

		public static int MapViewResource { get; set;}

		protected MapView MapView { get; set; }
		internal Projection BaseProjection { get; set; }
		protected TileLayer BaseLayer { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(ViewResource);

			MapView = (MapView)FindViewById(MapViewResource);

			BaseProjection = MapView.Options.BaseProjection;

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();
		}

		protected Carto.Graphics.Bitmap CreateBitmap(int resource)
		{
			return BitmapUtils.CreateBitmapFromAndroidBitmap(Android.Graphics.BitmapFactory.DecodeResource(Resources, resource));
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
