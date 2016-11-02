
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.DataSources.Components;
using Carto.Layers;
using Carto.Utils;
using CoreGraphics;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class CustomRasterDatasourceController : MapBaseController
	{
		public override string Name { get { return "Custom raster data source"; } }

		public override string Description { get { return "Creating and using custom raster tile data source"; } }

		const string TiledRasterUrl = "http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png";
		const string HillsideRasterUrl = "http://tiles.wmflabs.org/hillshading/{zoom}/{x}/{y}.png";

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

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
		HTTPTileDataSource source1, source2;

		public MergedRasterTileDataSource(HTTPTileDataSource source1, HTTPTileDataSource source2) : base(source1.MinZoom, source1.MaxZoom)
		{
			this.source1 = source1;
			this.source2 = source2;
		}

		public override Carto.DataSources.Components.TileData LoadTile(MapTile tile)
		{
			Carto.Graphics.Bitmap tileBitmap1 = CreateBitmap(source1, tile);
			Carto.Graphics.Bitmap tileBitmap2 = CreateBitmap(source2, tile);

			UIImage image1 = BitmapUtils.CreateUIImageFromBitmap(tileBitmap1);
			UIImage image2 = BitmapUtils.CreateUIImageFromBitmap(tileBitmap2);

			CGSize size = new CGSize(image1.CGImage.Width, image2.CGImage.Height);
			UIGraphics.BeginImageContext(size);

			image1.Draw(new CGRect(0, 0, size.Width, size.Height));
			image2.Draw(new CGRect(0, 0, size.Width, size.Height));

			UIImage final = UIGraphics.GetImageFromCurrentImageContext();

			UIGraphics.EndImageContext();

			BinaryData data = BitmapUtils.CreateBitmapFromUIImage(final).CompressToInternal();
			return new TileData(data);
		}

		Carto.Graphics.Bitmap CreateBitmap(HTTPTileDataSource source, MapTile tile)
		{
			return Carto.Graphics.Bitmap.CreateFromCompressed(source.LoadTile(tile).Data);
		}
	}
}