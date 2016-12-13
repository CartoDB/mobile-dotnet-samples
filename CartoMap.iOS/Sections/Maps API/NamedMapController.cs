using System;
using Carto.Core;
using Carto.Projections;
using Shared;
using Shared.iOS;

namespace CartoMap.iOS
{
	public class NamedMapController : MapBaseController
	{
		public override string Name { get { return "Named map"; } }

		public override string Description { get { return "CARTO data as vector tiles from a named map using VectorListener"; } }

		VectorTileListener listener;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Add base layer so we can attach a vector tile listener to it
			AddOnlineBaseLayer(Carto.Layers.CartoBaseMapStyle.CartoBasemapStyleGray);

			MapView.ConfigureNamedVectorLayers("tpl_69f3eebe_33b6_11e6_8634_0e5db1731f59");

			Projection projection = MapView.Options.BaseProjection;

			// Coordinates are available in the viz.json we download
			MapPos position = projection.FromLatLong(37.32549682016584, -121.94595158100128);
			MapView.FocusPos = position;
			MapView.SetZoom(17, 1);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			listener = MapView.InitializeVectorTileListener();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			if (listener != null)
			{
				// It'll never be null, if block simply to remove "is never used" warning
				listener = null;
			}
		}
	}
}

