
using System;
using Android.App;
using Shared.Droid;
using Shared;
using Android.Views;
using Android.Widget;
using Android.Content;
using Android.Graphics.Drawables;
using System.Collections.Generic;
using Carto.Ui;
using Carto.Layers;
using Carto.Core;
using Carto.VectorTiles;
using Carto.DataSources;
using Carto.Utils;
using Carto.Styles;

namespace AdvancedMap.Droid
{
	[Activity (ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	[ActivityData(Title = "Base maps", Description = "Overview of base maps offered by CARTO")]
	public class BaseMapsActivity : BaseActivity
	{
		BaseMapsView ContentView { get; set; }

		MapView MapView { get { return ContentView.MapView; } }

		VectorLayer VectorLayer { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new BaseMapsView(this);
			SetContentView(ContentView);

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

			Alert("Click the menu to choose between different styles and languages");

			ContentView.Menu.Items = Sections.List;

			// Set initial style 
			ContentView.Menu.SetInitialItem(Sections.Nutiteq);
			ContentView.Menu.SetInitialItem(Sections.Language);

			UpdateBaseLayer(Sections.Nutiteq, Sections.BaseStyleValue);
			UpdateLanguage(Sections.BaseLanguageCode);
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.Button.Click += OnMenuButtonClicked;
			ContentView.Menu.SelectionChange += OnMenuSelectionChanged;

			ContentView.Menu.Texts3D.Change += OnSwitchChange;
			ContentView.Menu.Buildings3D.Change += OnSwitchChange;
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.Button.Click -= OnMenuButtonClicked;
			ContentView.Menu.SelectionChange -= OnMenuSelectionChanged;

			ContentView.Menu.Texts3D.Change -= OnSwitchChange;
			ContentView.Menu.Buildings3D.Change -= OnSwitchChange;

			currentListener = null;
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

		void OnMenuButtonClicked(object sender, EventArgs e)
		{
			if (ContentView.Menu.IsVisible)
			{
				ContentView.Menu.Hide();
			}
			else {
				ContentView.Menu.Show();
				ContentView.Button.BringToFront();
			}
		}

		void OnMenuSelectionChanged(object sender, OptionEventArgs e)
		{
			UpdateBaseLayer(e.Section, e.Option.Value);
		}

		void OnSwitchChange(object sender, EventArgs e)
		{
			MapSwitch item = (MapSwitch)sender;
			UpdateBaseLayer(new Section { OSM = new NameValuePair { Value = currentOSM }, Type = SectionType.None }, currentSelection);
		}

		string currentOSM;
		string currentSelection;
		TileLayer currentLayer;

		VectorTileListener currentListener;

		void UpdateBaseLayer(Section section, string selection)
		{
			if (section.Type != SectionType.Language)
			{
				currentOSM = section.OSM.Value;
				currentSelection = selection;
			}

			if (section.Type == SectionType.Vector)
			{

				if (currentOSM == "nutiteq.osm")
				{
					// Nutiteq styles are bundled with the SDK, we can initialize them via constuctor
					if (currentSelection == "default")
					{
						currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
					}
					else if (currentSelection == "gray")
					{
						currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleGray);
					}
					else
					{
						currentLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDark);
					}
				}
				else if (currentOSM == "mapzen.osm")
				{
					// Mapzen styles are all bundled in one .zip file.
					// Selection contains both the style name and file name (cf. Sections.cs in Shared)
					string fileName = currentSelection.Split(':')[0];
					string styleName = currentSelection.Split(':')[1];

					// Create a style set from the file and style
					BinaryData styleAsset = AssetUtils.LoadAsset(fileName + ".zip");
					var package = new ZippedAssetPackage(styleAsset);
					CompiledStyleSet styleSet = new CompiledStyleSet(package, styleName);

					// Create datasource and style decoder
					var source = new CartoOnlineTileDataSource(currentOSM);
					var decoder = new MBVectorTileDecoder(styleSet);

					currentLayer = new VectorTileLayer(source, decoder);
				}

				ContentView.Menu.LanguageChoiceEnabled = true;
				ResetLanguage();
			}
			else if (section.Type == SectionType.Raster)
			{
				// We know that the value of raster will be Positron or Darkmatter,
				// as Nutiteq and Mapzen use vector tiles

				// Additionally, raster tiles do not support language choice
				string url = (currentSelection == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				currentLayer = new RasterTileLayer(source);

				// Language choice not enabled in raster tiles
				ContentView.Menu.LanguageChoiceEnabled = false;
			}
			else if (section.Type == SectionType.Language)
			{
				if (currentLayer is RasterTileLayer)
				{
					// Raster tile language chance is not supported
					return;
				}
				UpdateLanguage(selection);
			}
			else if (section.Type == SectionType.None)
			{
				// Switch was tapped
			}

			if (currentOSM == "nutiteq.osm")
			{
				var decoder = ((currentLayer as CartoOnlineVectorTileLayer).TileDecoder as MBVectorTileDecoder);
				MapSwitch texts = ContentView.Menu.Texts3D;
				MapSwitch buildings = ContentView.Menu.Buildings3D;

				decoder.SetStyleParameter(texts.ParameterName, texts.IsChecked.ToString());
				decoder.SetStyleParameter(buildings.ParameterName, buildings.IsChecked.ToString());

				decoder.SetStyleParameter(texts.ParameterName, texts.ParameterValue);
				decoder.SetStyleParameter(buildings.ParameterName, texts.ParameterValue);
			}

			MapView.Layers.Clear();
			MapView.Layers.Add(currentLayer);

			ContentView.Menu.Hide();

			currentListener = null;

			// Random if case to remove "unused variable" warning
			if (currentListener != null) currentListener.Dispose();

			currentListener = MapView.InitializeVectorTileListener(VectorLayer);
		}

		void ResetLanguage()
		{
			ContentView.Menu.SetInitialItem(Sections.Language);
			UpdateLanguage(Sections.BaseLanguageCode);
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

