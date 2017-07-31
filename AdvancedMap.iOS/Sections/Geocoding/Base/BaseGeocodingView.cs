
using System;
using Carto.Layers;
using Carto.Ui;
using UIKit;

namespace AdvancedMap.iOS
{
    public class BaseGeocodingView : UIView
    {
        public MapView MapView { get; private set; }

        public BaseGeocodingView()
        {
            MapView = new MapView();
            AddSubview(MapView);

            var layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            MapView.Layers.Add(layer);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            MapView.Frame = Bounds;
        }
    }
}
