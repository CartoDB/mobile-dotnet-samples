
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
                                                         Resource.Drawable.icon_wifi_off
                                                        )
        {
            GeocodingSource = new LocalVectorDataSource(Projection);
            var layer = new VectorLayer(GeocodingSource);
            MapView.Layers.Add(layer);

            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
        }

        public void ShowResult(GeocodingResult result, string title, string description, bool goToPosition)
        {
            GeocodingSource.Clear();

            var animationBuilder = new AnimationStyleBuilder();
            animationBuilder.RelativeSpeed = 2.0f;
            animationBuilder.FadeAnimationType = AnimationType.AnimationTypeSmoothstep;

            var balloonBuilder = new BalloonPopupStyleBuilder();
            balloonBuilder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
            balloonBuilder.TitleMargins = new BalloonPopupMargins(6, 3, 6, 3);
            balloonBuilder.CornerRadius = 5;
            balloonBuilder.AnimationStyle = animationBuilder.BuildStyle();
            // Make sure this label is shown on top of all others
            balloonBuilder.PlacementPriority = 10;

            FeatureCollection collection = result.FeatureCollection;
            int count = collection.FeatureCount;

            MapPos position = null;
            Geometry geometry = null;

            for (int i = 0; i < count; i++)
            {
                geometry = collection.GetFeature(i).Geometry;
                var color = new Carto.Graphics.Color(0, 100, 200, 150);

                var pointBuilder = new PointStyleBuilder();
                pointBuilder.Color = color;

                var lineBuilder = new LineStyleBuilder();
                lineBuilder.Color = color;

                var polygonBuilder = new PolygonStyleBuilder();
                polygonBuilder.Color = color;

                VectorElement element = null;

                if (geometry is PointGeometry)
                {
                    element = new Point((PointGeometry)geometry, pointBuilder.BuildStyle());
                }
                else if (geometry is LineGeometry)
                {
                    element = new Line((LineGeometry)geometry, lineBuilder.BuildStyle());
                }
                else if (geometry is PolygonGeometry)
                {
                    element = new Polygon((PolygonGeometry)geometry, polygonBuilder.BuildStyle());
                }
                else if (geometry is MultiGeometry)
                {
                    var collectionBuilder = new GeometryCollectionStyleBuilder();
                    collectionBuilder.PointStyle = pointBuilder.BuildStyle();
                    collectionBuilder.LineStyle = lineBuilder.BuildStyle();
                    collectionBuilder.PolygonStyle = polygonBuilder.BuildStyle();

                    element = new GeometryCollection((MultiGeometry)geometry, collectionBuilder.BuildStyle());
                }

                position = geometry.CenterPos;
                GeocodingSource.Add(element);
            }

            if (goToPosition)
            {
                MapView.SetFocusPos(position, 1.0f);
                MapView.SetZoom(17.0f, 1.0f);
            }

            var popup = new BalloonPopup(position, balloonBuilder.BuildStyle(), title, description);
            GeocodingSource.Add(popup);
        }
    }
}
