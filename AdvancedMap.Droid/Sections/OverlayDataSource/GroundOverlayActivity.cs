using System;
using Android.App;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Utils;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Ground overlay", Description = "Show non-tiled Bitmap on ground")]
	public class GroundOverlayActivity : MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Load ground overlay bitmap
			//Bitmap androidMarkerBitmap = BitmapFactory.decodeResource(getResources(), R.drawable.jefferson);
			//com.carto.graphics.Bitmap overlayBitmap = BitmapUtils.createBitmapFromAndroidBitmap(androidMarkerBitmap);
			Bitmap overlayBitmap = BitmapUtils.LoadBitmapFromAssets("jefferson-building-ground-floor.jpg");

			// Create two vectors containing geographical positions and corresponding raster image pixel coordinates.
			// 2, 3 or 4 points may be specified. Usually 2 points are enough (for conformal mapping).
			MapPos pos = BaseProjection.FromWgs84(new MapPos(-77.004590, 38.888702));
			double sizeNS = 110;
			double sizeWE = 100;

			MapPosVector mapPoses = new MapPosVector();
			mapPoses.Add(new MapPos(pos.X - sizeWE, pos.Y + sizeNS));
			mapPoses.Add(new MapPos(pos.X + sizeWE, pos.Y + sizeNS));
			mapPoses.Add(new MapPos(pos.X + sizeWE, pos.Y - sizeNS));
			mapPoses.Add(new MapPos(pos.X - sizeWE, pos.Y - sizeNS));

			ScreenPosVector bitmapPoses = new ScreenPosVector();
			bitmapPoses.Add(new ScreenPos(0, 0));
			bitmapPoses.Add(new ScreenPos(0, overlayBitmap.Height));
			bitmapPoses.Add(new ScreenPos(overlayBitmap.Width, overlayBitmap.Height));
			bitmapPoses.Add(new ScreenPos(overlayBitmap.Width, 0));

			// Create bitmap overlay raster tile data source
			BitmapOverlayRasterTileDataSource rasterDataSource = new BitmapOverlayRasterTileDataSource(
				0, 20, overlayBitmap, BaseProjection, mapPoses, bitmapPoses
			);

			RasterTileLayer rasterLayer = new RasterTileLayer(rasterDataSource);
			MapView.Layers.Add(rasterLayer);

			// Apply zoom level bias to the raster layer.
			// By default, bitmaps are upsampled on high-DPI screens.
			// We will correct this by applying appropriate bias
			float zoomLevelBias = (float)(Math.Log(MapView.Options.DPI / 160.0f) / Math.Log(2));

			rasterLayer.ZoomLevelBias = zoomLevelBias * 0.75f;
			rasterLayer.TileSubstitutionPolicy = TileSubstitutionPolicy.TileSubstitutionPolicyVisible;

			MapView.SetFocusPos(pos, 0);
			MapView.SetZoom(15.5f, 0);
		}
	}
}

