
using System;
using Android.Content;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Search;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class RouteSearchView : PackageDownloadBaseView
    {
        public VectorTileLayer BaseLayer { get; private set; }

        public RouteSearchView(Context context) : base(context,
                                                         Resource.Drawable.icon_info_blue,
                                                         Resource.Drawable.icon_back_blue,
                                                         Resource.Drawable.icon_close,
                                                         Resource.Drawable.icon_global,
                                                         Resource.Drawable.icon_wifi_on,
                                                         Resource.Drawable.icon_wifi_off,
                                                         Resource.Drawable.icon_forward_blue,
                                                         Resource.Drawable.icon_info_white
                                                        )
        {
            BaseLayer = AddBaseLayer(CartoBaseMapStyle.CartoBasemapStylePositron);

            var washingtonDC = Projection.FromWgs84(new MapPos(-77.0369, 38.9072));
            MapView.FocusPos = washingtonDC;
            MapView.Zoom = 14.0f;

            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
        }
    }
}
