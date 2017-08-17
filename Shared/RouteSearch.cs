using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Projections;
using Carto.Search;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
    public class RouteSearch
    {
		LocalVectorDataSource overlaySource;
		VectorLayer overlayLayer;

		LocalVectorDataSource popupSource;
		VectorLayer popupLayer;

		VectorTileSearchService searchService;

		VectorElementListener poiListener;

        Projection Projection;

        public RouteSearch(MapView MapView, VectorTileLayer BaseLayer)
        {
            Projection = MapView.Options.BaseProjection;

            MapPos washingtonDC = Projection.FromWgs84(new MapPos(-77.0369, 38.9072));
            MapView.FocusPos = washingtonDC;
            MapView.Zoom = 13.0f;

            overlaySource = new LocalVectorDataSource(Projection);
            overlayLayer = new VectorLayer(overlaySource);
            MapView.Layers.Add(overlayLayer);

            popupSource = new LocalVectorDataSource(Projection);
            popupLayer = new VectorLayer(popupSource);
            MapView.Layers.Add(popupLayer);

            poiListener = new VectorElementListener(popupSource);

            searchService = new VectorTileSearchService(BaseLayer.DataSource, BaseLayer.TileDecoder);
        }


        public void FindAttractions(FeatureCollection collection)
        {
            for (int i = 0; i < collection.FeatureCount; i++)
            {
                Feature feature = collection.GetFeature(i);

                if (feature.Geometry is LineGeometry)
                {
                    System.Threading.Tasks.Task.Run(delegate {
                        ShowAttractions(feature.Geometry);   
                    });
                }
            }
        }

        public void ShowAttractions(Geometry geometry)
        {
            var request = new SearchRequest();
            request.Projection = Projection;
            request.Geometry = geometry;
            request.SearchRadius = 500.0f;
            request.FilterExpression = "class='attraction'";

            var results = searchService.FindFeatures(request);

            for (int i = 0; i < results.FeatureCount; i++)
            {
                VectorTileFeature item = results.GetFeature(i);
                if (item.Geometry is PointGeometry)
                {
                    AddPointOfInterest(item);
                }
            }
        }

        public void AddPointOfInterest(VectorTileFeature item)
        {
            var builder = new PointStyleBuilder();
            builder.Size = 20.0f;
            builder.Color = new Carto.Graphics.Color(230, 100, 100, 200);

            MapPos position = (item.Geometry as PointGeometry).Pos;
            var point = new Point(position, builder.BuildStyle());

            point.SetMetaDataElement(VectorElementListener.RouteSearchTitle, new Variant("Properties"));
            point.SetMetaDataElement(VectorElementListener.RouteSearchDescription, new Variant(item.Properties.ToString()));

            overlaySource.Add(point);
        }

        public void AddListeners()
        {
            overlayLayer.VectorElementEventListener = poiListener;
        }

        public void RemoveListeners()
        {
            overlayLayer.VectorElementEventListener = null;
        }

        public void ClearPopups()
        {
            popupSource.Clear();
        }
	}
}
