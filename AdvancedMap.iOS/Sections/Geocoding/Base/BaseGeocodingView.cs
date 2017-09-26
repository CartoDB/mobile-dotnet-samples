
using Carto.DataSources;
using Carto.Layers;
using Carto.Ui;
using Shared.iOS;

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
