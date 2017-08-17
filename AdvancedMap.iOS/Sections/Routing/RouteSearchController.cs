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
        RouteSearch Search { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Routing.ShowTurns = false;

            Search = new RouteSearch(MapView, BaseLayer);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            MapListener.SingleTapped += OnSingleTap;

            Search.AddListeners();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            MapListener.SingleTapped += OnSingleTap;

            Search.RemoveListeners();
        }

        void OnSingleTap(object sender, EventArgs e)
        {
            Search.ClearPopups();
        }

        public override void RoutingComplete()
        {
            FeatureCollection collection = Routing.routeDataSource.GetFeatureCollection();
            Search.FindAttractions(collection);
        }

        protected override void SetBaseLayer()
        {
            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStylePositron);
        }
    }
}
