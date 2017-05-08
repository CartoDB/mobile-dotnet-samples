using System;
using System.Threading;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorTiles;
using Shared;
using Shared.iOS;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueShipController : MapBaseController
	{
		public override string Name { get { return "Torque Ship"; } }

		public override string Description { get { return "Indoor movement of shoppers throughout the day"; } }

		const long FRAMETIME = 100;

		TorqueTileDecoder decoder;
		TorqueTileLayer torqueLayer;

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

		Timer timer;

		TorqueView ContentView { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string username = "solutions";
			string mapname = "tpl_a108ee2b_6699_43bc_aa71_3b0bc962acf9";
			bool isVector = false;

			ContentView = new TorqueView();
			View = ContentView;

			ContentView.MapView.InitializeMapsService(username, mapname, isVector, delegate
			{
				InvokeOnMainThread(delegate
				{
					System.Console.WriteLine("Success: " + TorqueLayer);
					ContentView.Histogram.Initialize(decoder.FrameCount);
				});
			});

			MapPos center = ContentView.MapView.Options.BaseProjection.FromWgs84(new MapPos(0.0013, 0.0013));
			ContentView.MapView.FocusPos = center;
			ContentView.MapView.Zoom = 18.0f;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			timer = new Timer(new TimerCallback(UpdateTorque), null, FRAMETIME, FRAMETIME);

			ContentView.Histogram.Click += OnHistogramClicked;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

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

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			ContentView.Histogram.OnOrientationChange();
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

				InvokeOnMainThread(delegate
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

