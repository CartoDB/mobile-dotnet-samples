using System;
using System.Json;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Services;
using Shared;

namespace CartoMap.iOS
{
	public class CartoUTFGridController : VectorMapBaseController
	{
		public override string Name { get { return "Carto UTF Grid"; } }

		public override string Description { 
			get { 
				return "A sample demonstrating how to use Carto Maps API with Raster tiles and UTFGrid"; 
			} 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			JsonValue config = JsonUtils.UTFGridConfigJson;

			CartoMapUtils.ConfigureUTFGridLayers(MapView, config);

			// Animate map to the content area
			MapPos newYork = MapView.Options.BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}
	}
}

