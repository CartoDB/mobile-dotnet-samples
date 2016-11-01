
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

			Title = "Base maps";

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

			MapView.InitializeVectorTileListener(VectorLayer);

			Alert("Click the menu to choose between different styles and languages");

			ContentView.Menu.Items = Sections.List;
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.Button.Click += OnMenuButtonClicked;
			ContentView.Menu.SelectionChange += OnMenuSelectionChanged;
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.Button.Click -= OnMenuButtonClicked;
			ContentView.Menu.SelectionChange -= OnMenuSelectionChanged;
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

		string currentOSM;
		string currentSelection;
		TileLayer currentLayer;

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
					// MapZen styles are not, styles need to manually added to assets and then decoded
					BinaryData styleAsset = AssetUtils.LoadAsset(currentSelection + ".zip");
					currentLayer = new CartoOnlineVectorTileLayer(currentOSM, new ZippedAssetPackage(styleAsset));
				}
			}
			else if (section.Type == SectionType.Raster)
			{
				// We know that the value of raster will be Positron or Darkmatter,
				// as Nutiteq and Mapzen use vector tiles

				// Additionally, raster tiles do not support language choice
				string url = (currentSelection == "positron") ? Urls.Positron : Urls.DarkMatter;

				TileDataSource source = new HTTPTileDataSource(1, 19, url);
				currentLayer = new RasterTileLayer(source);
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

			MapView.Layers.Clear();
			MapView.Layers.Add(currentLayer);

			ContentView.Menu.Hide();
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

