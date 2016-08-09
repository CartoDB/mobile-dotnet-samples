
using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;

namespace CartoMobileSample
{
	public class VectorMapBaseController : MapBaseController
	{
		const string Menu_Language = "Language";
		const string Menu_Style = "Style";

		public const string BaseStyle = "nutibright-v2a";
		public const string BaseStyleFile = BaseStyle + ".zip";

		const string BaseLanguage = "en";

		protected TileDataSource vectorTileDataSource;
		protected MBVectorTileDecoder vectorTileDecoder;

		// Style Parameters
		protected string vectorStyleName = BaseStyle; // default style name, each style has corresponding .zip asset
		protected string vectorStyleLang = BaseLanguage; // default map language

		Dictionary<string, string> styleDict = new Dictionary<string, string> {
			{ "Basic", "basic" },
			{ "NutiBright 2D", "nutibright-v2a" },
			{ "NutiBright 3D", "ntibright3d" },
			{ "Loose Leaf", "looseleaf" }
		};

		Dictionary<string, string> languageDict = new Dictionary<string, string> {
			{ "English", "en" },
			{ "German", "de" },
			{ "Spanish", "es" },
			{ "Italian", "it" },
			{ "French", "fr" },
			{ "Russian", "ru" },
			{ "Chinese", "zh" }
		};

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView.Options.ZoomRange = new MapRange(0, 20);

			UpdateBaseLayer();
		}

		void UpdateBaseLayer()
		{
			string styleAssetName = vectorStyleName + ".zip";
			bool styleBuildings3D = false;

			if (vectorStyleName.Equals("nutibright3d"))
			{
				styleAssetName = BaseStyleFile;
				styleBuildings3D = true;
			}

			BinaryData styleBytes = AssetUtils.LoadAsset(styleAssetName);

			if (styleBytes == null)
			{
				Alert("Map style file must be in project assets: ");
				return;
			}

			// Create style set
			CompiledStyleSet vectorTileStyleSet = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));
			vectorTileDecoder = new MBVectorTileDecoder(vectorTileStyleSet);

			// Set language, language-specific texts from vector tiles will be used
			vectorTileDecoder.SetStyleParameter("lang", vectorStyleLang);

			// OSM Bright style set supports choosing between 2d/3d buildings. Set corresponding parameter.
			if (styleAssetName.Equals(BaseStyleFile))
			{
				vectorTileDecoder.SetStyleParameter("buildings3d", styleBuildings3D ? "1" : "0");
				vectorTileDecoder.SetStyleParameter("markers3d", styleBuildings3D ? "1" : "0");
				vectorTileDecoder.SetStyleParameter("texts3d", styleBuildings3D ? "1" : "0");
			}

			// Create tile data source for vector tiles
			if (vectorTileDataSource == null)
			{
				vectorTileDataSource = CreateTileDataSource();
			}

			// Remove old base layer, create new base layer
			if (BaseLayer != null)
			{
				ContentView.Layers.Remove(BaseLayer);
			}

			BaseLayer = new VectorTileLayer(vectorTileDataSource, vectorTileDecoder);
			ContentView.Layers.Insert(0, BaseLayer);
		}

		protected virtual TileDataSource CreateTileDataSource()
		{
			TileDataSource source = new CartoOnlineTileDataSource("nutiteq.osm");

			// We don't use vectorTileDataSource directly (this would be also option),
			// but via caching to cache data locally persistently/non-persistently
			// Note that persistent cache requires WRITE_EXTERNAL_STORAGE permission
			return new MemoryCacheTileDataSource(source);
		}
	}
}

