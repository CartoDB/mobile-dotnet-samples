using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace CartoMobileSample
{
	[Activity]
	public class CartoVisJSONActivity : BaseMapActivity
	{
		const string _base = "https://documentation.cartodb.com/api/v2/viz/";

		Dictionary<string, string> items = new Dictionary<string, string> {
			{ "Circle", _base + "836e37ca-085a-11e4-8834-0edbca4b5057/viz.json" },
			{ "Test", _base + "3ec995a8-b6ae-11e4-849e-0e4fddd5de28/viz.json" },
			{ "Countries", _base + "2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json" },
			{ "Dots", _base + "236085de-ea08-11e2-958c-5404a6a683d5/viz.json" }
		};

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// MapSampleBaseActivity creates and configures mapView
			base.OnCreate(savedInstanceState);

			// Load the initial visJSON
			string url = items["Circle"];
			UpdateVis(url);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			foreach (KeyValuePair<string, string> item in items) {
				menu.Add(item.Key);
			}

			return true;
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			Console.WriteLine(featureId + " - " + item);

			string key = item.TitleFormatted.ToString();
			string url = items[key];

			UpdateVis(url);

			return base.OnMenuItemSelected(featureId, item);
		}

		protected void UpdateVis(string url)
		{
			ThreadPool.QueueUserWorkItem(delegate
			{
				MapView.Layers.Clear();

				// Create overlay layer for Popups
				Projection projection = MapView.Options.BaseProjection;
				LocalVectorDataSource source = new LocalVectorDataSource(projection);
				VectorLayer layer = new VectorLayer(source);

				// Create VIS loader
				CartoVisLoader loader = new CartoVisLoader();
				loader.DefaultVectorLayerMode = true;
				MyCartoVisBuilder builder = new MyCartoVisBuilder(MapView, layer);

				try
				{
					loader.LoadVis(builder, url);
				}
				catch (Exception e)
				{
					Toast.MakeText(this, e.Message, ToastLength.Short);
				}

				MapView.Layers.Add(layer);
			});
		}

		#region INTERNAL CLASSES

		class MyCartoVisBuilder : CartoVisBuilder
		{
			VectorLayer vectorLayer; // vector layer for popups
			MapView mapView;

			public MyCartoVisBuilder(MapView mapView, VectorLayer vectorLayer)
			{
				this.mapView = mapView;
				this.vectorLayer = vectorLayer;
			}

			public override void SetCenter(MapPos mapPos)
			{
				mapView.SetFocusPos(mapView.Options.BaseProjection.FromWgs84(mapPos), 1.0f);
			}

			public override void SetZoom(float zoom)
			{
				mapView.SetZoom(zoom, 1.0f);
			}

			public override void AddLayer(Layer layer, Variant attributes)
			{
				// Add the layer to the map view
				mapView.Layers.Add(layer);

				// Check if the layer has info window. In that case will add a custom UTF grid event listener to the layer.
				Variant infoWindow = attributes.GetObjectElement("infowindow");

				if (infoWindow.Type == VariantType.VariantTypeObject)
				{
					TileLayer tileLayer = (TileLayer)layer;
					tileLayer.UTFGridEventListener = new MyUTFGridEventListener(vectorLayer, infoWindow); ;
				}
			}
		}

		class MyUTFGridEventListener : UTFGridEventListener
		{
			VectorLayer vectorLayer;
			//Variant infoWindow;

			public MyUTFGridEventListener(VectorLayer vectorLayer, Variant infoWindow)
			{
				this.vectorLayer = vectorLayer;
				//this.infoWindow = infoWindow;
			}

			public override bool OnUTFGridClicked(UTFGridClickInfo clickInfo)
			{
				LocalVectorDataSource vectorDataSource = (LocalVectorDataSource)vectorLayer.DataSource;

				// Clear previous popups
				vectorDataSource.Clear();

				// Multiple vector elements can be clicked at the same time, we only care about the one
				// Check the type of vector element
				BalloonPopup clickPopup = null;
				BalloonPopupStyleBuilder styleBuilder = new BalloonPopupStyleBuilder();

				// Configure style
				styleBuilder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
				styleBuilder.TitleMargins = new BalloonPopupMargins(6, 3, 6, 3);

				// Make sure this label is shown on top all other labels
				styleBuilder.PlacementPriority = 10;

				// Show clicked element variant as JSON string
				string desc = clickInfo.ElementInfo.ToString();

				clickPopup = new BalloonPopup(clickInfo.ClickPos, styleBuilder.BuildStyle(), "Clicked", desc);

				vectorDataSource.Add(clickPopup);

				return true;
			}
		}

		#endregion
	}
}

