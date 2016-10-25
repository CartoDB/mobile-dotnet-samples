
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

namespace AdvancedMap.Droid
{
	[Activity(Label = "")]
	[ActivityDescription(Description = "Choice of different Base Maps")]
	public class BaseMapsActivity : MapBaseActivity
	{
		BaseMapsView ContentView { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);


			ContentView = new BaseMapsView(this);
			SetContentView(ContentView);
		}

		protected override void OnPause()
		{
			base.OnPause();

			ContentView.Button.Click += OnStyleChanged;
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.Button.Click -= OnStyleChanged;
		}

		void OnStyleChanged(object sender, EventArgs e)
		{
			
		}
	}
}

