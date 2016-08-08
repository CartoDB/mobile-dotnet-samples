using System;
using Carto.Core;
using Carto.Layers;
using Carto.Services;
using Carto.Ui;

namespace CartoMobileSample
{
	public class MyCartoVisBuilder : CartoVisBuilder
	{
		VectorLayer vectorLayer; // vector layer for popups
		MapView mapView;

		public MyCartoVisBuilder(MapView mapView, VectorLayer vectorLayer)
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

}

