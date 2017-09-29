
using Android.App;
using Android.Graphics;
using Android.OS;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    [ActivityData(Title = "User Data", Description = "Bundled package data displayed on the map with CartoCSS")]
    public class BundledPackageDataActivity : MapBaseActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BinaryData bytes = AssetUtils.LoadAsset("carto-fonts.zip");
            ZippedAssetPackage package = new ZippedAssetPackage(bytes);

            string css = JsonUtils.OfflinePackageCartoCSS;
            var decoder = new MBVectorTileDecoder(new CartoCSSStyleSet(css, package));

            TileDataSource source = FileUtils.CreateTileDataSource(this, "offline-packages.mbtiles");

            var layer = new VectorTileLayer(source, decoder);
            MapView.Layers.Add(layer);
        }

    }
}
