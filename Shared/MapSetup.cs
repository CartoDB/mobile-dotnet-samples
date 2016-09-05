using System;
using Carto.Core;
using Carto.Graphics;
using Carto.Ui;
using Carto.Utils;
using Carto.Layers;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Projections;
using Carto.Styles;
using Carto.PackageManager;
using Carto.VectorTiles;
using Carto.Geometry;
using System.Threading.Tasks;

namespace Shared
{
	public class MapSetup
	{
		// Set base projection
		// From wiki.openstreetmap: EPSG:3857 is a Spherical Mercator projection coordinate system 
		// popularized by web services such as Google and later OpenStreetMap
		public static EPSG3857 proj = new EPSG3857();

		public static void InitLocation(IMapView mapView)
		{
			// Set initial location and other parameters, don't animate
			mapView.FocusPos = proj.FromWgs84(new MapPos(-0.8164, 51.2383)); // Berlin
			mapView.Zoom = 2;
			mapView.MapRotation = 0;
			mapView.Tilt = 90;
		}

		public static void InitializePackageManager(string packageFolder, 
			string importPackagePath, IMapView mapView, string downloadedPackage)
		{
			// Offline base layer

			// Define listener, definition is in same class above
			var packageManager = new CartoPackageManager("nutiteq.osm", packageFolder);
			packageManager.PackageManagerListener = new PackageListener(packageManager, downloadedPackage);

			// Download new package list only if it is older than 24h
			// Note: this is only needed if pre-made packages are used
			if (packageManager.ServerPackageListAge > 24 * 60 * 60)
			{
				packageManager.StartPackageListDownload();
			}

			// Start manager - mandatory
			packageManager.Start();

			// Import initial package
			if (packageManager.GetLocalPackage("world0_4") == null)
			{
				packageManager.StartPackageImport("world0_4", 1, importPackagePath);
			}

			// Now can add vector map as layer
			// Define styling for vector map
			BinaryData styleBytes = AssetUtils.LoadAsset("nutibright-v2a.zip");
			MBVectorTileDecoder vectorTileDecoder = null;

			if (styleBytes != null)
			{
				// Create style set
				var vectorTileStyleSet = new CompiledStyleSet(new ZippedAssetPackage(styleBytes));
				vectorTileDecoder = new MBVectorTileDecoder(vectorTileStyleSet);
			}
			else {
				Log.Error("Failed to load style data");
			}

			// Create online base layer (no package download needed then). Use vector style from assets (osmbright.zip)
			// comment in to use online map. Packagemanager stuff is not needed then

			//VectorTileLayer baseLayer = new NutiteqOnlineVectorTileLayer("osmbright.zip");

			var baseLayer = new VectorTileLayer(new PackageManagerTileDataSource(packageManager), vectorTileDecoder);
			mapView.Layers.Add(baseLayer);
		}


		public static void StartBboxDownload(CartoPackageManager packageManager)
		{
			// Bounding box download can be done now
			// For the country package download see OnPackageListUpdated in PackageListener

			//string bbox = "bbox(-0.8164,51.2382,0.6406,51.7401)"; // London (approx. 30MB)
			string bbox = "bbox(-73.581182, 41.013, -73.5811, 41.013503)";

			if (packageManager.GetLocalPackage(bbox) == null)
			{
				packageManager.StartPackageDownload(bbox);
			}
		}


		async public static void AddMapOverlays(IMapView mapView)
		{
			// Create overlay layer for markers
			var dataSource = new LocalVectorDataSource(proj);
			var overlayLayer = new VectorLayer(dataSource);
			mapView.Layers.Add(overlayLayer);

			// Create line style, and line poses
			var lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Width = 8;

			var linePoses = new MapPosVector();

			// proj.FromWgs84 returns (spherical) Mercator projection
			linePoses.Add(proj.FromWgs84(new MapPos(0, 0)));
			linePoses.Add(proj.FromWgs84(new MapPos(0, 80)));
			linePoses.Add(proj.FromWgs84(new MapPos(45, 45)));

			var line = new Line(linePoses, lineStyleBuilder.BuildStyle());
			dataSource.Add(line);

			// Create balloon popup
			var balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			balloonPopupStyleBuilder.CornerRadius = 3;
			balloonPopupStyleBuilder.TitleFontName = "Helvetica";
			balloonPopupStyleBuilder.TitleFontSize = 55;
			balloonPopupStyleBuilder.TitleColor = new Color(200, 0, 0, 255);
			balloonPopupStyleBuilder.StrokeColor = new Color(200, 120, 0, 255);
			balloonPopupStyleBuilder.StrokeWidth = 1;
			balloonPopupStyleBuilder.PlacementPriority = 1;

			var popup = new BalloonPopup(
				proj.FromWgs84(new MapPos(0, 20)),
				balloonPopupStyleBuilder.BuildStyle(),
				"Title", "Description");

			dataSource.Add(popup);

			// Load NML file model from a file
			BinaryData modelFile = AssetUtils.LoadAsset("fcd_auto.nml");

			// set location for model, and create NMLModel object with this
			var modelPos = proj.FromWgs84(new MapPos(24.646469, 59.423939));
			var model = new NMLModel(modelPos, modelFile);
			mapView.FocusPos = modelPos;
			mapView.Zoom = 15;

			// Oversize it 20*, just to make it more visible (optional)
			model.Scale = 20;

			// Add metadata for click handling (optional)
			model.SetMetaDataElement("ClickText", new Variant("Single model"));

			// Add it to normal datasource
			dataSource.Add(model);

			// Create and set map listener
			mapView.MapEventListener = new MapListener(dataSource);

			await AnimateModel(model);
		}

		public static async Task AnimateModel(NMLModel model)
		{
			for (int i = 0; i < 3600; i++)
			{
				model.SetRotation(new MapVec(0, 0, 1), i);
				await Task.Delay(10);
			}
		}


		public static void AddJsonLayer(IMapView mapView, string json)
		{
			var features = new Carto.Geometry.GeoJSONGeometryReader().ReadFeatureCollection(json);

			//var geoJsonParser = new GeoJSONGeometryReader();

			var balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();

			// Create overlay layer for markers
			var dataSource = new LocalVectorDataSource(proj);
			var overlayLayer = new ClusteredVectorLayer(dataSource, new MyClusterElementBuilder());
			overlayLayer.MinimumClusterDistance = 80; // in pixels

			mapView.Layers.Add(overlayLayer);

			for (int i = 0; i < features.FeatureCount; i++)
			{
				var geom = features.GetFeature(i).Geometry;
				var props = features.GetFeature(i).Properties;

				// Create popup
				var popup = new BalloonPopup(
					geom,
					balloonPopupStyleBuilder.BuildStyle(),
					props.GetObjectElement("Capital").String, props.GetObjectElement("Country").String);

				// Copy feature properties to popup metadata
				for (int j = 0; j < props.ObjectKeys.Count; j++)
				{
					var key = props.ObjectKeys[j];
					var val = props.GetObjectElement(key);
					popup.SetMetaDataElement(key, val);
				}

				dataSource.Add(popup);
			}
		}

	}
}


