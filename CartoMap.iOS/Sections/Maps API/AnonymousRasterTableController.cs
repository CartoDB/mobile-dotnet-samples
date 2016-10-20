using System;
using System.IO;
using Carto.Core;
using Carto.Layers;
using Carto.Services;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class AnonymousRasterTableController : MapBaseController
	{
		public override string Name { get { return "Anonymous Raster Tile"; } }

		public override string Description { get { return "Using Carto PostGIS Raster data"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// You need to change these according to your DB
			string sql = "select * from table_46g";
			string cartoCSS = "#table_46g {raster-opacity: 0.5;}";

			string config = JsonUtils.GetRasterLayerConfigJson(sql, cartoCSS).ToString();

			// Use the Maps service to configure layers. Note that this must be done
			// in a separate thread on Android, as Maps API requires connecting to server
			// which is not allowed in main thread.

			CartoMapsService mapsService = new CartoMapsService();
			mapsService.Username = "nutiteq";

			// Use raster layers, not vector layers
			mapsService.DefaultVectorLayerMode = false;

			try
			{
				LayerVector layers = mapsService.BuildMap(Variant.FromString(config));
				for (int i = 0; i < layers.Count; i++)
				{
					MapView.Layers.Add(layers[i]);
				}
			}
			catch (IOException e)
			{
				Carto.Utils.Log.Debug("EXCEPTION: Exception: " + e);
			}

			// Zoom map to the content area
			MapPos hiiumaa = BaseProjection.FromWgs84(new MapPos(22.7478235498916, 58.8330577553785));
			MapView.SetFocusPos(hiiumaa, 0);
			MapView.SetZoom(11, 0);
		}
	
	}
}

