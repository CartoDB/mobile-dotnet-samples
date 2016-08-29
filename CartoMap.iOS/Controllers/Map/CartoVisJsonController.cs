
using System;
using System.Collections.Generic;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Shared;

namespace CartoMap.iOS
{
	public class CartoVisJsonController : MapBaseController
	{
		public override string Name { get { return "Carto VisJson"; } }

		public override string Description {
			get {
				return "High level Carto VisJSON API to display interactive maps";
			}
		}

		const string _base = "https://documentation.cartodb.com/api/v2/viz/";

		Dictionary<string, string> items = new Dictionary<string, string> {
			{ "Circle", _base + "836e37ca-085a-11e4-8834-0edbca4b5057/viz.json" },
			{ "Test", _base + "3ec995a8-b6ae-11e4-849e-0e4fddd5de28/viz.json" },
			{ "Countries", _base + "2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json" },
			{ "Dots", _base + "236085de-ea08-11e2-958c-5404a6a683d5/viz.json" }
		};

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			string url = items["Dots"];
			UpdateVis(url);
		}

		protected void UpdateVis(string url)
		{
			InvokeInBackground(delegate
			{
				MapView.Layers.Clear();

				// Create overlay layer for Popups
				Projection projection = MapView.Options.BaseProjection;
				LocalVectorDataSource source = new LocalVectorDataSource(projection);
				VectorLayer layer = new VectorLayer(source);

				// Create VIS loader
				CartoVisLoader loader = new CartoVisLoader();
				loader.DefaultVectorLayerMode = true;
				CartoVisBuilderWithGridEvent builder = new CartoVisBuilderWithGridEvent(MapView, layer);

				try
				{
					loader.LoadVis(builder, url);
				}
				catch (Exception e)
				{
					Console.WriteLine("Exception: " + e.Message);
				}

				MapView.Layers.Add(layer);
			});
		}
	}
}

