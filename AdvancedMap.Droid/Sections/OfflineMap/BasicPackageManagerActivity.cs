
using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Basic Package Manager", Description = "Download a bounding box of London")]
	public class BasicPackageManagerActivity : BaseActivity
	{
		BasicPackageManagerView ContentView;

		CartoPackageManager manager;

		PackageListener updateListener = new PackageListener();

		BoundingBox bbox;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();

			ContentView = new BasicPackageManagerView(this);
			SetContentView(ContentView);

			string folder = CreateFolder("mappackages");

			manager = new CartoPackageManager("nutiteq.osm", folder);
			manager.PackageManagerListener = updateListener;

			// Custom convience class to enhance readability 
			bbox = new BoundingBox { MinLon = -0.8164, MinLat = 51.2382, MaxLon = 0.6406, MaxLat = 51.7401 };

			if (manager.GetLocalPackage(bbox.ToString()) == null)
			{
				manager.StartPackageDownload(bbox.ToString());
			}
			else {
				UpdateStatusLabel("Package downloaded");
				ContentView.ZoomTo(bbox.Center);
			}

			ContentView.SetBaseLayer(new PackageManagerTileDataSource(manager));
		}

		protected override void OnResume()
		{
			base.OnResume();

			// Always Attach handlers OnResume to avoid memory leaks and objects with multple handlers
			updateListener.OnPackageCancel += UpdatePackage;
			updateListener.OnPackageUpdate += UpdatePackage;
			updateListener.OnPackageStatusChange += UpdatePackage;
			updateListener.OnPackageFail += UpdatePackage;

			manager.Start();
		}

		protected override void OnPause()
		{
			// Always detach handlers OnPause to avoid memory leaks and objects with multple handlers
			updateListener.OnPackageCancel -= UpdatePackage;
			updateListener.OnPackageUpdate -= UpdatePackage;
			updateListener.OnPackageStatusChange -= UpdatePackage;
			updateListener.OnPackageFail -= UpdatePackage;

			manager.Stop(true);

			base.OnPause();
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdateStatusLabel("Downloaded Complete");
			ContentView.ZoomTo(bbox.Center);
		}

		void UpdatePackage(object sender, PackageStatusEventArgs e)
		{
			UpdateStatusLabel("Progress: " + e.Status.Progress + "%");
		}

		void UpdatePackage(object sender, PackageFailedEventArgs e)
		{
			UpdateStatusLabel("Failed: " + e.ErrorType);
		}

		string CreateFolder(string name)
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), name);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}

		void UpdateStatusLabel(string text)
		{
			RunOnUiThread(delegate { ContentView.StatusLabel.Text = text; });
		}
	}

	public class BasicPackageManagerView : RelativeLayout
	{
		public MapView MapView { get; private set; }

		public TextView StatusLabel { get; private set; }

		public BasicPackageManagerView(Context context) : base(context)
		{
			MapView = new MapView(context);
			MapView.LayoutParameters = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			AddView(MapView);

			// Initialize & style Status label
			StatusLabel = new TextView(context);
			StatusLabel.SetTextColor(Color.Black);

			GradientDrawable background = new GradientDrawable();
			background.SetCornerRadius(5);
			background.SetColor(Color.Argb(160, 255, 255, 255));
			StatusLabel.Background = background;

			StatusLabel.Gravity = Android.Views.GravityFlags.Center;
			StatusLabel.Typeface = Typeface.Create("HelveticaNeue", TypefaceStyle.Normal);

			DisplayMetrics screen = Resources.DisplayMetrics;

			int width = screen.WidthPixels / 2;
			int height = width / 4;

			int x = screen.WidthPixels / 2 - width / 2;
			int y = screen.HeightPixels / 100;

			var parameters = new RelativeLayout.LayoutParams(width, height);
			parameters.TopMargin = y;
			parameters.LeftMargin = x;

			AddView(StatusLabel, parameters);
		}

		public void ZoomTo(MapPos position)
		{
			MapView.FocusPos = MapView.Options.BaseProjection.FromWgs84(position);
			MapView.SetZoom(12, 2);
		}

		public void SetBaseLayer(PackageManagerTileDataSource source)
		{
			// Create style set
			BinaryData styleBytes = AssetUtils.LoadAsset("nutiteq-dark.zip");
			var style = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));

			// Create Decoder
			var decoder = new MBVectorTileDecoder(style);

			var layer = new VectorTileLayer(source, decoder);

			MapView.Layers.Add(layer);
		}
	}
}
