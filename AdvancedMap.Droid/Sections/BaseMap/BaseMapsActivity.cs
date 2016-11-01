
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

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Base maps", Description = "Overview of base maps offered by CARTO")]
	public class BaseMapsActivity : Activity
	{
		BaseMapsView ContentView { get; set; }

		MapView MapView { get { return ContentView.MapView; } }

		VectorLayer VectorLayer { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new BaseMapsView(this);
			SetContentView(ContentView);

			// Zoom to Central Europe so some texts would be visible
			MapPos europe = MapView.Options.BaseProjection.FromWgs84(new MapPos(15.2551, 54.5260));
			MapView.SetFocusPos(europe, 0);
			MapView.Zoom = 5;

			MapView.InitializeVectorTileListener(VectorLayer);
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.Button.Click += OnMenuClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.Button.Click -= OnMenuClicked;
		}

		void OnMenuClicked(object sender, EventArgs e)
		{
			Console.WriteLine("OnMenuClicked");
		}
	}
}

