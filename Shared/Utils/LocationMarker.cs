
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared.Utils
{
    /*
     * LocationMarker: shared class, implemententing only CartoMobileSDK's common APIs,
     * used to create a point of the user's location and show an accuracy polygon around it.
     * 
     * All layer creation and adding logic is done within this class, 
     * and there is no reordering logic. Make sure this layer is on top of everything else.
     */
    public class LocationMarker
    {
        MapView map;

        LocalVectorDataSource source;
        Projection projection;

        public LocationMarker(MapView map)
        {
            this.map = map;

            projection = map.Options.BaseProjection;
            source = new LocalVectorDataSource(projection);

            var layer = new VectorLayer(source);
            map.Layers.Add(layer);
        }

        Point userMarker;
        Polygon accuracyMarker;

        bool focus = true;

        public void ShowAt(double latitude, double longitude, float accuracy)
        {
            var positiion = projection.FromWgs84(new MapPos(longitude, latitude));

            if (focus)
            {
                map.FocusPos = positiion;
                map.Zoom = 16;
            }

            var builder = new PolygonStyleBuilder();
            // Light transparent apple blue
            builder.Color = new Carto.Graphics.Color(14, 122, 254, 70);
            var borderBuilder = new LineStyleBuilder();
            // Dark transparent apple blue
            borderBuilder.Color = new Carto.Graphics.Color(14, 122, 254, 150);
            borderBuilder.Width = 1;
            builder.LineStyle = borderBuilder.BuildStyle();

            var points = GetCirclePoints(latitude, longitude, accuracy);

            if (accuracyMarker == null)
            {
                var holes = new MapPosVectorVector();
                accuracyMarker = new Polygon(points, holes, builder.BuildStyle());
                source.Add(accuracyMarker);
            }
            else
            {
                accuracyMarker.Style = builder.BuildStyle();
                accuracyMarker.Geometry = new PolygonGeometry(points);
            }

            if (userMarker == null)
            {
                var pointBuilder = new PointStyleBuilder();
                // Apple blue
                pointBuilder.Color = new Carto.Graphics.Color(14, 122, 254, 255);
                pointBuilder.Size = 16.0f;

                userMarker = new Point(positiion, pointBuilder.BuildStyle());
                source.Add(userMarker);
            }
        }

        MapPosVector GetCirclePoints(double centerLat, double centerLon, float radius)
		{
			// Number of points of circle
			int N = 100;
			int EARTH_RADIUS = 6378137;

			MapPosVector points = new MapPosVector();

			for (int i = 0; i <= N; i++)
			{
				double angle = Math.PI * 2 * (i % N) / N;
				double dx = radius * Math.Cos(angle);
				double dy = radius * Math.Sin(angle);
				double lat = centerLat + (180 / Math.PI) * (dy / EARTH_RADIUS);
				double lon = centerLon + (180 / Math.PI) * (dx / EARTH_RADIUS) / Math.Cos(centerLat * Math.PI / 180);
                points.Add(projection.FromWgs84(new MapPos(lon, lat)));
			}

			return points;
		}
    }
}
