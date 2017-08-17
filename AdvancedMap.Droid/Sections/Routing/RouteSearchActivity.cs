
using System;
using Android.App;
using Carto.Geometry;
using Carto.Layers;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    [Activity]
    [ActivityData(Title = "Route Search", Description = "Finds points of interest near your route")]
    public class RouteSearchActivity : OnlineRoutingActivity
    {
        RouteSearch Search { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

			Routing.ShowTurns = false;

			Search = new RouteSearch(MapView, BaseLayer);
        }

        protected override void OnResume()
        {
            base.OnResume();

			MapListener.SingleTapped += OnSingleTap;

			Search.AddListeners();
        }

        protected override void OnPause()
        {
            base.OnPause();

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
