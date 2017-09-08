
using System;
using Shared.Droid;
using Android.Content;
using Carto.DataSources;
using Carto.Layers;
using Carto.Geocoding;
using Carto.Styles;
using Carto.Geometry;
using Carto.Core;
using Carto.VectorElements;

namespace AdvancedMap.Droid
{
    public class BaseGeocodingView : PackageDownloadBaseView
    {
        public const string Source = "geocoding:carto.streets";

        public LocalVectorDataSource GeocodingSource { get; private set; }

        public BaseGeocodingView(Context context) : base(context, 
                                                         Resource.Drawable.icon_info_blue, 
                                                         Resource.Drawable.icon_back_blue, 
                                                         Resource.Drawable.icon_close, 
                                                         Resource.Drawable.icon_global, 
                                                         Resource.Drawable.icon_wifi_on, 
                                                         Resource.Drawable.icon_wifi_off,
                                                         Resource.Drawable.icon_forward_blue
                                                        )
        {
            GeocodingSource = new LocalVectorDataSource(Projection);
            var layer = new VectorLayer(GeocodingSource);
            MapView.Layers.Add(layer);
        }

    }
}
