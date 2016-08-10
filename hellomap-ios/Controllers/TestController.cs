
using System;
using System.Threading;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Projections;
using Carto.Routing;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;
using Carto.VectorTiles;
using Foundation;
using UIKit;

namespace CartoMobileSample
{
	public class TestController : MapBaseController
	{
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			#region base

			//Projection proj = MapView.Options.BaseProjection;

			//// Initialize an local vector data source
			//LocalVectorDataSource vectorDataSource1 = new LocalVectorDataSource(proj);

			//// Initialize a vector layer with the previous data source
			//VectorLayer vectorLayer1 = new VectorLayer(vectorDataSource1);

			//// Add the previous vector layer to the map
			//MapView.Layers.Add(vectorLayer1);

			//// Set limited visible zoom range for the vector layer
			//vectorLayer1.VisibleZoomRange = new MapRange(10, 24);

			#endregion
		}

	}
}

