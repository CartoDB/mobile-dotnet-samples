
using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Geocoding;
using Carto.Geometry;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class BaseGeocodingView : PackageDownloadBaseView
    {
        public LocalVectorDataSource ObjectSource { get; private set; }

        public BaseGeocodingView()
        {
            var layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            MapView.Layers.Add(layer);

            ObjectSource = new LocalVectorDataSource(Projection);
            var objectLayer = new VectorLayer(ObjectSource);
            MapView.Layers.Add(objectLayer);
        }

    }
}
