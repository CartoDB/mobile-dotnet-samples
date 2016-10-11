using System;
using System.Collections.Generic;
using System.Linq;
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
using Shared.Droid;
using Shared;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "High level Carto VisJSON API to display interactive maps")]
	public class CartoVisJsonActivity : BaseMapActivity
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
			string url = items["Dots"];
			MapView.UpdateVisWithGridEvent(url, OnError);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			foreach (KeyValuePair<string, string> item in items)
			{
				menu.Add(item.Key);
			}

			return true;
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			Console.WriteLine(featureId + " - " + item);

			if (item.ItemId == Android.Resource.Id.Home) {
				return base.OnMenuItemSelected(featureId, item);
			}

			string key = item.TitleFormatted.ToString();
			string url = items[key];

			MapView.UpdateVisWithGridEvent(url, OnError);

			return base.OnMenuItemSelected(featureId, item);
		}

		void OnError(string message)
		{
			Toast.MakeText(this, message, ToastLength.Long).Show();
		}

	}
}

