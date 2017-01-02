using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
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

			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

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
			MapPos washington = BaseProjection.FromWgs84(new MapPos(-77.0369, 38.9072));
			Marker marker = new Marker(washington, style);
			source.Add(marker);

			// Animate map to the marker
			MapView.SetFocusPos(washington, 1);
			MapView.SetZoom(8, 1);

			listener = new RenderListener();
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

			listener.ScreenCaptured -= OnScreenCaptured;
		}

		protected override void OnResume()
		{
			base.OnResume();

			listener.ScreenCaptured += OnScreenCaptured;
		}

		void OnScreenCaptured(object sender, ScreenshotEventArgs e)
		{
			this.bitmap = e.Bitmap;

			if (((int)Build.VERSION.SdkInt) >= Marshmallow)
			{
				Android.Support.V4.App.ActivityCompat.RequestPermissions(
					this,
					new string[] { Android.Manifest.Permission.WriteExternalStorage, Android.Manifest.Permission.ReadExternalStorage },
					RequestCode
				);
			}
			else {
				OnPermissionGranted();
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			if (requestCode == RequestCode)
			{
				if (grantResults[0] == Android.Content.PM.Permission.Granted)
				{
					OnPermissionGranted();
					
					return;
				}

				Finish();
			}
		}

		Carto.Graphics.Bitmap bitmap;
		MapPos position = new MapPos();
		int number = 0;

		void OnPermissionGranted()
		{
			if (!MapView.FocusPos.Equals(position))
			{
				position = MapView.FocusPos;
				number++;

				Android.Graphics.Bitmap image = BitmapUtils.CreateAndroidBitmapFromBitmap(bitmap);

				string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string filename = number + ".png";

				string path = Path.Combine(folder, filename);

				string message = null;

				try
				{
					using (var stream = new FileStream(path, FileMode.Create))
					{
						image.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 100, stream);
					}
				}
				catch (Exception e)
				{
					message = e.Message;
				}


				Share(path);

				if (message == null)
				{
					Alert("Great success! Screenshot saved to: " + path);
				}
				else {
					Alert("Error! " + message);
				}
			}
		}

		void Share(string path)
		{
			Intent intent = new Intent(Intent.ActionSend);
			intent.SetType("image/png");

			intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(path));

			StartActivity(Intent.CreateChooser(intent, "Share image"));
		}
	}

	public class RenderListener : RendererCaptureListener
	{
		public EventHandler<ScreenshotEventArgs> ScreenCaptured;

		public override void OnMapRendered(Carto.Graphics.Bitmap bitmap)
		{
			if (ScreenCaptured != null)
			{
				ScreenshotEventArgs args = new ScreenshotEventArgs { Bitmap = bitmap };
				ScreenCaptured(this, args);
			}
		}
	}

	public class RenderedEventArgs : EventArgs
	{
		Carto.Graphics.Bitmap Bitmap { get; set; }
	}
}

