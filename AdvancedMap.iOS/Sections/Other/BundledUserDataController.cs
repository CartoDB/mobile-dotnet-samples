
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class BundledUserDataController : MapBaseController
    {
		public override string Name { get { return "User Data"; } }

		public override string Description { get { return "Bundled user data displayed on the map with CartoCSS"; } }

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			BinaryData bytes = AssetUtils.LoadAsset("carto-fonts.zip");
			ZippedAssetPackage package = new ZippedAssetPackage(bytes);

			string css = JsonUtils.OfflinePackageCartoCSS;
			var decoder = new MBVectorTileDecoder(new CartoCSSStyleSet(css, package));

			TileDataSource source = FileUtils.CreateTileDataSource("offline-packages", "mbtiles");

			var layer = new VectorTileLayer(source, decoder);
			MapView.Layers.Add(layer);
        }
    }
}
