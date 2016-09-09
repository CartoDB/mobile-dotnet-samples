using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;

namespace CartoMap.WindowsPhone
{
    // TODO: Use Common Code
    public static class MapExtensions
    {
        public static async void UpdateVisWithGridEvent(this MapView map, string url, Action<string> error = null)
        {
            await ThreadPool.RunAsync(delegate
            {
                map.Layers.Clear();

                // Create overlay layer for Popups
                Projection projection = map.Options.BaseProjection;
                LocalVectorDataSource source = new LocalVectorDataSource(projection);
                VectorLayer layer = new VectorLayer(source);

                // Create VIS loader
                CartoVisLoader loader = new CartoVisLoader();
                loader.DefaultVectorLayerMode = true;
                CartoVisBuilderWithGridEvent builder = new CartoVisBuilderWithGridEvent(map, layer);

                try
                {
                    loader.LoadVis(builder, url);
                }
                catch (Exception e)
                {
                    if (error != null)
                    {
                        error(e.Message);
                    }
                }

                map.Layers.Add(layer);
            });
        }
    }

    public class CartoVisBuilderWithGridEvent : CartoVisBuilder
    {
        VectorLayer vectorLayer; // vector layer for popups
        MapView mapView;

        public CartoVisBuilderWithGridEvent(MapView mapView, VectorLayer vectorLayer)
        {
            this.mapView = mapView;
            this.vectorLayer = vectorLayer;
        }

        public override void SetCenter(MapPos mapPos)
        {
            mapView.SetFocusPos(mapView.Options.BaseProjection.FromWgs84(mapPos), 1.0f);
        }

        public override void SetZoom(float zoom)
        {
            mapView.SetZoom(zoom, 1.0f);
        }

        public override void AddLayer(Layer layer, Variant attributes)
        {
            // Add the layer to the map view
            mapView.Layers.Add(layer);

            // Check if the layer has info window. In that case will add a custom UTF grid event listener to the layer.
            Variant infoWindow = attributes.GetObjectElement("infowindow");

            if (infoWindow.Type == VariantType.VariantTypeObject)
            {
                TileLayer tileLayer = (TileLayer)layer;
                tileLayer.UTFGridEventListener = new MyUTFGridEventListener(vectorLayer, infoWindow);
            }
        }
    }

    public class MyUTFGridEventListener : UTFGridEventListener
    {
        VectorLayer vectorLayer;
        LocalVectorDataSource vectorDataSource;

        public MyUTFGridEventListener(VectorLayer vectorLayer, Variant infoWindow = null)
        {
            this.vectorLayer = vectorLayer;
        }

        public MyUTFGridEventListener(LocalVectorDataSource source)
        {
            vectorDataSource = source;
        }

        public override bool OnUTFGridClicked(UTFGridClickInfo clickInfo)
        {
            if (vectorDataSource == null)
            {
                vectorDataSource = (LocalVectorDataSource)vectorLayer.DataSource;
            }

            // Clear previous popups
            vectorDataSource.Clear();

            // Multiple vector elements can be clicked at the same time, we only care about the one
            // Check the type of vector element
            BalloonPopup clickPopup = null;
            BalloonPopupStyleBuilder styleBuilder = new BalloonPopupStyleBuilder();

            // Configure style
            styleBuilder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
            styleBuilder.TitleMargins = new BalloonPopupMargins(6, 3, 6, 3);

            // Make sure this label is shown on top all other labels
            styleBuilder.PlacementPriority = 10;

            // Show clicked element variant as JSON string
            string desc = clickInfo.ElementInfo.ToString();

            clickPopup = new BalloonPopup(clickInfo.ClickPos, styleBuilder.BuildStyle(), "Clicked", desc);

            vectorDataSource.Add(clickPopup);

            return true;
        }
    }
}
