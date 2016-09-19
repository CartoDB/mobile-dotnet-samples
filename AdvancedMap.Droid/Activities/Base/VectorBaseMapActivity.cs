
using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	public class VectorBaseMapActivity : BaseMapActivity
	{
		const string Menu_Language = "Language";
		const string Menu_Style = "Style";

		public const string BaseStyle = "nutibright-v2a";
		public const string BaseStyleFile = BaseStyle + ".zip";

		const string BaseLanguage = "en";

		protected TileDataSource vectorTileDataSource;
		protected MBVectorTileDecoder vectorTileDecoder;

		protected bool persistentTileCache = false;

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

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView.Options.ZoomRange = new MapRange(0, 20);

			UpdateBaseLayer();
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				return base.OnMenuItemSelected(featureId, item);
			}

			string key = item.TitleFormatted.ToString();

			if (key == Menu_Language || key == Menu_Style)
			{
				// Parent menu click - Do nothing
			}
			else {
				
				if (styleDict.ContainsKey(key))
				{
					vectorStyleName = styleDict[key];
				}
				else if (languageDict.ContainsKey(key))
				{
					vectorStyleLang = languageDict[key];
				}

				UpdateBaseLayer();
			}

			return base.OnMenuItemSelected(featureId, item);
		}

		public override bool OnCreateOptionsMenu(Android.Views.IMenu menu)
		{
			ISubMenu langMenu = menu.AddSubMenu(Menu_Language.ToCharSequence());

			foreach (KeyValuePair<string, string> language in languageDict) 
			{
				langMenu.Add(language.Key.ToCharSequence());
			}

			ISubMenu styleMenu = menu.AddSubMenu(Menu_Style.ToCharSequence());

			foreach (KeyValuePair<string, string> style in styleDict)
			{
				styleMenu.Add(style.Key.ToCharSequence());
			}

			return true;
		}

		protected virtual void UpdateBaseLayer()
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
				Toast.MakeText(this, "Map style file must be in project assets: " + vectorStyleName, ToastLength.Short).Show();
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
				MapView.Layers.Remove(BaseLayer);
			}

			BaseLayer = new VectorTileLayer(vectorTileDataSource, vectorTileDecoder);
			MapView.Layers.Insert(0, BaseLayer);
		}

		protected virtual TileDataSource CreateTileDataSource()
		{
			TileDataSource vectorTileDataSource = new CartoOnlineTileDataSource("nutiteq.osm");

			// We don't use vectorTileDataSource directly (this would be also option),
			// but via caching to cache data locally persistently/non-persistently
			// Note that persistent cache requires WRITE_EXTERNAL_STORAGE permission
			TileDataSource cacheDataSource = vectorTileDataSource;

			if (persistentTileCache)
			{
				string cacheFile = GetExternalFilesDir(null) + "/mapcache.db";
				cacheDataSource = new PersistentCacheTileDataSource(vectorTileDataSource, cacheFile);
			}
			else {
				cacheDataSource = new MemoryCacheTileDataSource(vectorTileDataSource);
			}

			return cacheDataSource;
		}
	}
}

