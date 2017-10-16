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
		public override string Name { get { return "Raster Layer from CARTO"; } }

		public override string Description { get { return "PostGIS Raster data from Maps API"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStylePositron);

			// You need to change these according to your DB
			string sql = "select * from table_46g";
			string cartoCSS = "#table_46g { raster-opacity: 0.5; }";
			string config = JsonUtils.GetRasterLayerConfigJson(sql, cartoCSS).ToString();

			CartoMapsService mapsService = new CartoMapsService();
			mapsService.Username = "nutiteq";

			// Use raster layers, not vector layers
			mapsService.DefaultVectorLayerMode = false;

			// Use the Maps service to configure layers. 
			// Note that Maps API requires connecting to server,
			// which shouldn't be done on the main thread

			InvokeInBackground(delegate
			{
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
			});
			                   
			// Zoom map to the content area
			MapPos hiiumaa = BaseProjection.FromWgs84(new MapPos(22.7478235498916, 58.8330577553785));
			MapView.SetFocusPos(hiiumaa, 0);
			MapView.SetZoom(10, 0);
		}
	
	}
}

