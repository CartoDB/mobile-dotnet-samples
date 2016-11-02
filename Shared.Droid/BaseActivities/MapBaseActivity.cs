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

		protected void AddBaseLayer(CartoBaseMapStyle withStyle)
		{
			// Initialize map
			var baseLayer = new CartoOnlineVectorTileLayer(withStyle);
			MapView.Layers.Add(baseLayer);
		}
	}
}
