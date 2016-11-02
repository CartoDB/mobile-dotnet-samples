
using System;
using Android.App;
using Android.Graphics;
using Carto.Core;
using Carto.DataSources;
using Carto.DataSources.Components;
using Carto.Layers;
using Carto.Utils;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Custom raster data source", Description = "Creating and using custom raster tile data source")]
	public class CustomRasterDatasourceActivity : MapBaseActivity
	{
		const string TiledRasterUrl = "http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png";
		const string HillsideRasterUrl = "http://tiles.wmflabs.org/hillshading/{zoom}/{x}/{y}.png";

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Initialize hillshading raster data source, better visible in mountain ranges
			HTTPTileDataSource source1 = new HTTPTileDataSource(0, 24, TiledRasterUrl);
			HTTPTileDataSource source2 = new HTTPTileDataSource(0, 24, HillsideRasterUrl);

			MergedRasterTileDataSource mergedSource = new MergedRasterTileDataSource(source1, source2);

			var layer = new RasterTileLayer(mergedSource);
			MapView.Layers.Add(layer);

			// Animate map to a nice place
			MapView.SetFocusPos(BaseProjection.FromWgs84(new MapPos(-122.4323, 37.7582)), 1);
			MapView.SetZoom(13, 1);
		}
	}

	public class MergedRasterTileDataSource : TileDataSource
	{
		Paint paint;

		HTTPTileDataSource source1, source2;

		public MergedRasterTileDataSource(HTTPTileDataSource source1, HTTPTileDataSource source2) : base(source1.MinZoom, source1.MaxZoom)
		{
			this.source1 = source1;
			this.source2 = source2;

			paint = new Paint();
		}

		public override Carto.DataSources.Components.TileData LoadTile(MapTile tile)
		{
			Carto.Graphics.Bitmap tileBitmap1 = CreateBitmap(source1, tile);
			Carto.Graphics.Bitmap tileBitmap2 = CreateBitmap(source2, tile);

			Bitmap image1 = BitmapUtils.CreateAndroidBitmapFromBitmap(tileBitmap1);
			Bitmap image2 = BitmapUtils.CreateAndroidBitmapFromBitmap(tileBitmap2);

			Canvas canvas = new Canvas(image1);
			canvas.DrawBitmap(image2, null, new Rect(0, 0, image1.Height, image1.Width), paint);

			BinaryData data = BitmapUtils.CreateBitmapFromAndroidBitmap(image1).CompressToInternal();
			return new TileData(data);
		}

		Carto.Graphics.Bitmap CreateBitmap(HTTPTileDataSource source, MapTile tile)
		{
			return Carto.Graphics.Bitmap.CreateFromCompressed(source.LoadTile(tile).Data);
		}
	}
}

