using System;
using System.Json;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Services;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class AnonymousVectorTableController : MapBaseController
	{
		public override string Name { get { return "Anonymous Vector Tile"; } }

		public override string Description { 
			get { 
				return "Usage of Carto Maps API with vector tiles"; 
			} 
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleGray);

			JsonValue config = JsonUtils.VectorLayerConfigJson;

			MapView.ConfigureAnonymousVectorLayers(config);

			// Animate map to the content area
			MapPos newYork = MapView.Options.BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}
	}
}

