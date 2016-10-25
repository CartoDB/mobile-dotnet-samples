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

namespace Shared.Droid
{
	[Activity (Icon = "@mipmap/icon")]
	public class MapBaseActivity : Activity
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

			BaseProjection = new EPSG3857();

			// Initialize map
			var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(baseLayer);

			Title = GetType().GetTitle();

			ActionBar.SetDisplayHomeAsUpEnabled(true);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
