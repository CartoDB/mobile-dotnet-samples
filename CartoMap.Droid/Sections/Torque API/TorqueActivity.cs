
using System;
using System.Threading;
using Android.App;
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
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Torque Ship", Description = "Indoor movement throughout the day")]
	public class TorqueShipsActivity : BaseActivity
	{
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

			ContentView.MapView.InitializeMapsService(username, mapname, isVector);

			MapPos center = ContentView.MapView.Options.BaseProjection.FromWgs84(new MapPos(0.0013, 0.0013));
			ContentView.MapView.FocusPos = center;
			ContentView.MapView.Zoom = 18.0f;
		}

		protected override void OnStart()
		{
			base.OnStart();

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);
		}

		protected override void OnStop()
		{
			base.OnStop();

			timer.Dispose();
			timer = null;
		}

		void UpdateTorque(object state)
		{
			if (ContentView.Button.IsPaused)
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
					ContentView.Counter.Update(frameNumber, decoder.FrameCount);
				});
			});
		}
	}
}

