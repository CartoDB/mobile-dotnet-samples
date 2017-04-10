
using System;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorTiles;
using Java.IO;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity(ScreenOrientation = ScreenOrientation.Landscape | ScreenOrientation.ReverseLandscape)]
	[ActivityData(Title = "Torque Ship", Description = "Indoor movement throughout the day")]
	public class TorqueShipsActivity : BaseActivity
	{
	
	// Loads and shows Torque map: https://team.carto.com/u/solutions/builder/a108ee2b-6699-43bc-aa71-3b0bc962acf9/embed
	
		const long FRAMETIME = 100;

		TorqueTileDecoder decoder;
		TorqueTileLayer torqueLayer;

		Timer timer;

		TorqueTileLayer TorqueLayer
		{
			get
			{
				if (torqueLayer != null)
				{
					return torqueLayer;
				}

				for (int i = 0; i < ContentView.MapView.Layers.Count; i++)
				{
					Layer layer = ContentView.MapView.Layers[i];
					if (layer is TorqueTileLayer)
					{
						torqueLayer = (TorqueTileLayer)layer;
						decoder = (TorqueTileDecoder)torqueLayer.TileDecoder;
						return torqueLayer;
					}
				}

				return null;
			}
		}

		TorqueView ContentView { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			string username = "solutions";
			string mapname = "tpl_a108ee2b_6699_43bc_aa71_3b0bc962acf9";
			bool isVector = false;

			ContentView = new TorqueView(this);
			SetContentView(ContentView);

			ContentView.MapView.InitializeMapsService(username, mapname, isVector, delegate {

				RunOnUiThread(delegate
				{
					System.Console.WriteLine("Success: " + TorqueLayer);
					ContentView.InitializeHistogram(decoder.FrameCount);
				});
			});

			MapPos center = ContentView.MapView.Options.BaseProjection.FromWgs84(new MapPos(0.0013, 0.0013));
			ContentView.MapView.FocusPos = center;
			ContentView.MapView.Zoom = 18.0f;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			ContentView.Dispose();
		}

		protected override void OnResume()
		{
			base.OnResume();

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);

			ContentView.Histogram.Click += OnHistogramClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			timer.Dispose();
			timer = null;

			ContentView.Histogram.Click -= OnHistogramClicked;
		}

		void OnHistogramClicked(object sender, HistogramEventArgs e)
		{
			ContentView.Histogram.Button.Pause();
			ContentView.Histogram.Counter.Update(e.FrameNumber);
			TorqueLayer.FrameNr = e.FrameNumber;
		}

		int max;

		void UpdateTorque(object state)
		{
			if (ContentView.Histogram.Button.IsPaused)
			{
				return;
			}

			if (TorqueLayer == null)
			{
				return;
			}

			System.Threading.Tasks.Task.Run(delegate
			{
				int frameNumber = (TorqueLayer.FrameNr + 1) % decoder.FrameCount;
				TorqueLayer.FrameNr = frameNumber;

				RunOnUiThread(delegate
				{
					int count = TorqueLayer.CountVisibleFeatures(frameNumber);

					if (count > max)
					{
						max = count;
						ContentView.Histogram.UpdateAll(max);
					}
					else
					{
						ContentView.Histogram.UpdateElement(frameNumber, count, max);
					}

					ContentView.Histogram.Counter.Update(frameNumber, decoder.FrameCount);
				});
			});
		}
	}
}

