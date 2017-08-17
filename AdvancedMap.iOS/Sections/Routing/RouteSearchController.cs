using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Search;
using Carto.Styles;
using Carto.VectorElements;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class RouteSearchController : OnlineRoutingController
    {
        LocalVectorDataSource overlaySource;
        VectorLayer overlayLayer;

		LocalVectorDataSource popupSource;
		VectorLayer popupLayer;

        VectorTileSearchService searchService;

        VectorElementListener poiListener;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Routing.ShowTurns = false;

            MapPos washingtonDC = BaseProjection.FromWgs84(new MapPos(-77.0369, 38.9072));
            MapView.FocusPos = washingtonDC;
            MapView.Zoom = 13.0f;

            overlaySource = new LocalVectorDataSource(BaseProjection);
            overlayLayer = new VectorLayer(overlaySource);
            MapView.Layers.Add(overlayLayer);

			popupSource = new LocalVectorDataSource(BaseProjection);
			popupLayer = new VectorLayer(popupSource);
            MapView.Layers.Add(popupLayer);

            poiListener = new VectorElementListener(popupSource);

            searchService = new VectorTileSearchService(BaseLayer.DataSource, BaseLayer.TileDecoder);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MapListener.SingleTapped += OnSingleTap;

            overlayLayer.VectorElementEventListener = poiListener;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            MapListener.SingleTapped += OnSingleTap;

            overlayLayer.VectorElementEventListener = null;
        }

        void OnSingleTap(object sender, EventArgs e)
        {
            popupSource.Clear();
        }

        public override void RoutingComplete()
        {
            FeatureCollection collection = Routing.routeDataSource.GetFeatureCollection();

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
			request.Projection = BaseProjection;
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

        protected override void SetBaseLayer()
        {
            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
        }
    }
}
