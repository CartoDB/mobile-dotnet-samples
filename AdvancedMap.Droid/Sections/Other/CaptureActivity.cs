using System;
using System.IO;
using Android.App;
using Android.Content;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Renderers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Screencapture", Description = "Captures rendered MapView as a Bitmap")]
	public class CaptureActivity : MapBaseActivity
	{
		RenderListener listener;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

			// Initialize a local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(source);

			// Add the previous vector layer to the map
			MapView.Layers.Add(layer);
			// Set visible zoom range for the vector layer
			layer.VisibleZoomRange = new MapRange(0, 18);

			// Create marker style
			Android.Graphics.Bitmap image = Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.marker);
			Carto.Graphics.Bitmap markerBitmap = BitmapUtils.CreateBitmapFromAndroidBitmap(image);

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

			listener = new RenderListener(this, MapView);
			MapView.MapRenderer.CaptureRendering(listener, true);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			listener = null;
		}

		protected override void OnPause()
		{
			base.OnPause();

			listener.ScreenCaptured -= OnScreenCapture;
		}

		protected override void OnResume()
		{
			base.OnResume();

			listener.ScreenCaptured += OnScreenCapture;
		}

		void OnScreenCapture(object sender, ScreenshotEventArgs e)
		{
			if (e.IsOK)
			{
				Alert("Great success! Screenshot saved to: " + e.Path);
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

		Activity context;

		public RenderListener(Activity context, MapView map)
		{
			this.context = context;
			this.map = map;
		}

		public override void OnMapRendered(Carto.Graphics.Bitmap bitmap)
		{
			if (!map.FocusPos.Equals(position))
			{
				position = map.FocusPos;
				number++;

				Android.Graphics.Bitmap image = BitmapUtils.CreateAndroidBitmapFromBitmap(bitmap);

				string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string filename = number + "png";

				string path = Path.Combine(folder, filename);

				string message = null;

				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						image.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
					}
				}
				catch(Exception e) {
					message = e.Message;
				}

				if (ScreenCaptured != null)
				{
					ScreenshotEventArgs args = new ScreenshotEventArgs { Path = path, Message = message };
					ScreenCaptured(this, args);
				}

				Share(path);
			}
		}

		void Share(string path)
		{
			Intent intent = new Intent(Intent.ActionSend);
			intent.SetType("image/png");

			intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(path));

			context.StartActivity(Intent.CreateChooser(intent, "Share image"));
		}

	}
}

