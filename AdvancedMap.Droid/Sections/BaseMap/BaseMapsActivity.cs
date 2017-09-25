
using System;
using Android.App;
using Shared.Droid;
using Shared;
using Android.Views;
using Carto.Ui;
using Carto.Layers;
using Carto.Core;
using Carto.VectorTiles;
using Carto.DataSources;
using Carto.Utils;
using Carto.Styles;
using AdvancedMap.Droid.Sections.BaseMap.Views;
using AdvancedMap.Droid.Sections.BaseMap.Subviews;

namespace AdvancedMap.Droid
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	[ActivityData(Title = "Base maps", Description = "Overview of base maps offered by CARTO")]
	public class BaseMapsActivity : BaseActivity
	{
		Sections.BaseMap.Views.BaseMapsView ContentView { get; set; }

		MapView MapView { get { return ContentView.MapView; } }

		VectorLayer VectorLayer { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new Sections.BaseMap.Views.BaseMapsView(this);
			SetContentView(ContentView);

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

            currentLayer = ContentView.AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            ContentView.StyleContent.HighlightDefault();
		}

		protected override void OnResume()
		{
			base.OnResume();

            ContentView.BasemapButton.Clicked += OnBasemapButtonClick;

            foreach (var section in ContentView.StyleContent.Sections)
            {
                foreach (var item in section.List)
                {
                    item.Click += OnStyleItemClick;
                }    
            }

		}

        protected override void OnPause()
		{
			base.OnPause();

            ContentView.BasemapButton.Clicked -= OnBasemapButtonClick;

            foreach (var section in ContentView.StyleContent.Sections)
            {
                foreach (var item in section.List)
                {
                    item.Click -= OnStyleItemClick;
                }    
            }

			currentListener = null;
		}

        void OnBasemapButtonClick(object sender, EventArgs e)
        {
            ContentView.Popup.SetPopupContent(ContentView.StyleContent);
            ContentView.Popup.Show();
        }

		void OnStyleItemClick(object sender, EventArgs e)
		{
            ContentView.Popup.Hide();

            if (ContentView.StyleContent.Previous != null)
            {
                ContentView.StyleContent.Previous.Normalize();
            }

            var item = (StylePopupContentSectionItem)sender;
            item.Highlight();

            string selection = item.Label.Text;
            string source = (item.Parent as StylePopupContentSection).Source;
            UpdateBaseLayer(selection, source);

            ContentView.StyleContent.Previous = item;
		}

		string currentOSM;
		string currentSelection;
		TileLayer currentLayer;

		VectorTileListener currentListener;

        void UpdateBaseLayer(string selection, string source)
        {
            if (source.Equals(StylePopupContent.CartoVectorSource))
            {
                // Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
                if (selection.Equals(StylePopupContent.Voyager))
                {
                    currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
                }
                else if (selection.Equals(StylePopupContent.Positron))
                {
                    currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
                }
                else
                {
                    currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDarkmatter);
                }
            }
            else if (source.Equals(StylePopupContent.MapzenSource))
            {
                // Mapzen styles are all bundled in one .zip file.
                // Selection contains both the style name and file name (cf. Sections.cs in Shared)

                // Create a style set from the file and style
                BinaryData styleAsset = AssetUtils.LoadAsset("styles_mapzen.zip");
                var package = new ZippedAssetPackage(styleAsset);

                string name = "";

                if (selection.Equals(StylePopupContent.Bright))
                {
                    // The name of the actual style is "bright", as it's displayed in the UI,
                    // but in the style file it's a default "style", change the name
                    name = "style";    
                } 
                else if (selection.Equals(StylePopupContent.Positron))
                {
                    name = "positron";
                }
                else if (selection.Equals(StylePopupContent.DarkMatter))
                {
                    name = "positron_dark";
                }

                CompiledStyleSet styleSet = new CompiledStyleSet(package, name);

                // Create datasource and style decoder
                var ds = new CartoOnlineTileDataSource(source);
                var decoder = new MBVectorTileDecoder(styleSet);

                currentLayer = new VectorTileLayer(ds, decoder);

                //ContentView.Menu.LanguageChoiceEnabled = true;
                //ResetLanguage();

            }
            else if (source.Equals(StylePopupContent.CartoRasterSource))
            {
                // We know that the value of raster will be Positron or Darkmatter,
                // as Nutiteq and Mapzen use vector tiles

                // Additionally, raster tiles do not support language choice
                string url = (selection == StylePopupContent.Positron) ? Urls.Positron : Urls.DarkMatter;

                TileDataSource ds = new HTTPTileDataSource(1, 19, url);
                currentLayer = new RasterTileLayer(ds);

                // Language choice not enabled in raster tiles
                //ContentView.Menu.LanguageChoiceEnabled = false;
            }

            MapView.Layers.Clear();
            MapView.Layers.Add(currentLayer);

            //ContentView.Menu.Hide();

            currentListener = null;

            // Random if case to remove "unused variable" warning
            if (currentListener != null) currentListener.Dispose();

            currentListener = MapView.InitializeVectorTileListener(VectorLayer);
        }

		void ResetLanguage()
		{
			//ContentView.Menu.SetInitialItem(Shared.Sections.Language);
			UpdateLanguage(Shared.Sections.BaseLanguageCode);
		}

		void UpdateLanguage(string code)
		{
			if (currentLayer == null)
			{
				return;
			}

			MBVectorTileDecoder decoder = (currentLayer as VectorTileLayer).TileDecoder as MBVectorTileDecoder;
			decoder.SetStyleParameter("lang", code);
		}

	}
}

