using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Renderers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;
using Foundation;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class CaptureController : MapBaseController
	{
		public override string Name { get { return "Screencapture"; } }

		public override string Description { get { return "Captures rendered MapView as a Bitmap"; } }

		RenderListener listener;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Initialize a local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(source);

			// Add the previous vector layer to the map
			MapView.Layers.Add(layer);
			// Set visible zoom range for the vector layer
			layer.VisibleZoomRange = new MapRange(0, 18);

			// Create marker style
			UIImage image = UIImage.FromFile("marker.png");
			Bitmap markerBitmap = BitmapUtils.CreateBitmapFromUIImage(image);

			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Bitmap = markerBitmap;
			builder.Size = 30;
			MarkerStyle style = builder.BuildStyle();

			// Add marker
			MapPos berlin = BaseProjection.FromWgs84(new MapPos(13.38933, 52.51704));
			Marker marker = new Marker(berlin, style);
			source.Add(marker);

			// Animate map to the marker
			MapView.SetFocusPos(berlin, 1);
			MapView.SetZoom(12, 1);

			listener = new RenderListener(MapView);
			MapView.MapRenderer.CaptureRendering(listener, true);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			listener.ScreenCaptured += OnScreenCapture;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			listener.ScreenCaptured -= OnScreenCapture;
		}

		void OnScreenCapture(object sender, ScreenshotEventArgs e)
		{
			if (e.IsOK)
			{
				Alert("Great success! Screenshot saved as: " + e.Path);
			}
			else {
				Alert("Error! " + e.Message);
			}
		}
	}

	public class RenderListener : RendererCaptureListener
	{
		public EventHandler<ScreenshotEventArgs> ScreenCaptured;

		MapPos position = new MapPos();
		int number = 0;

		MapView map;

		public RenderListener(MapView map)
		{
			this.map = map;
		}

		public override void OnMapRendered(Bitmap bitmap)
		{
			if (!map.FocusPos.Equals(position))
			{
				position = map.FocusPos;
				number++;

				UIImage image = BitmapUtils.CreateUIImageFromBitmap(bitmap);

				string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string filename = number + "png";

				string path = System.IO.Path.Combine(folder, filename);

				NSData data = image.AsPNG();
				NSError error;

				bool success = data.Save(path, false, out error);

				ScreenshotEventArgs args = new ScreenshotEventArgs { Path = path };

				if (success)
				{
					ScreenCaptured(this, args);
				}
				else {
					args.Message = error.LocalizedDescription;
					ScreenCaptured(this, args);
				}
			}
		}
	}

	public class ScreenshotEventArgs : EventArgs
	{
		public string Path { get; set; }

		public string Message { get; set; }

		public bool IsOK { get { return string.IsNullOrWhiteSpace(Message); } }
	}
}

