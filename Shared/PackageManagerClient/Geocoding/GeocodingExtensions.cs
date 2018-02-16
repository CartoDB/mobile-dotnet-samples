
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geocoding;
using Carto.Geometry;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
    public static class GeocodingExtensions
    {
        public static string GetPrettyAddress(this GeocodingResult result)
        {
            var parsed = "";
            var address = result.Address;

            if (address.Name.IsNotEmpty())
            {
                parsed += address.Name;
            }

            if (address.Street.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Street;
            }

            if (address.HouseNumber.IsNotEmpty())
            {
                parsed += " " + address.HouseNumber;
            }

            if (address.Neighbourhood.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Neighbourhood;
            }

            if (address.Locality.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Locality;
            }

            if (address.County.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.County;
            }

            if (address.Region.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Region;
            }

            if (address.Country.IsNotEmpty())
            {
                parsed += parsed.AddCommaIfNecessary();
                parsed += address.Country;
            }

            return parsed;
        }

        public static string AddCommaIfNecessary(this string original)
        {
            if (original.Length > 0)
            {
                return ", ";
            }

            return "";
        }

        public static bool IsNotEmpty(this string original)
        {
            return !string.IsNullOrWhiteSpace(original);
        }


		public static void ShowResult(this LocalVectorDataSource source, MapView map, GeocodingResult result, string title, string description, bool goToPosition)
		{
			source.Clear();

			var builder = new BalloonPopupStyleBuilder();
			builder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
			builder.TitleMargins = new BalloonPopupMargins(6, 3, 6, 3);
			builder.CornerRadius = 5;
			// Make sure this label is shown on top of all other labels
			builder.PlacementPriority = 10;

			FeatureCollection collection = result.FeatureCollection;
			int count = collection.FeatureCount;

            MapPos position = new MapPos();
			Geometry geometry;

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
                    element = new Point(geometry as PointGeometry, pointBuilder.BuildStyle());
                }
                else if (geometry is LineGeometry)
                {
                    element = new Line(geometry as LineGeometry, lineBuilder.BuildStyle());
                }
                else if (geometry is PolygonGeometry)
                {
                    element = new Polygon(geometry as PolygonGeometry, polygonBuilder.BuildStyle());
                }
                else if (geometry is MultiGeometry)
                {
                    var collectionBuilder = new GeometryCollectionStyleBuilder();
                    collectionBuilder.PointStyle = pointBuilder.BuildStyle();
                    collectionBuilder.LineStyle = lineBuilder.BuildStyle();
                    collectionBuilder.PolygonStyle = polygonBuilder.BuildStyle();

                    element = new GeometryCollection(geometry as MultiGeometry, collectionBuilder.BuildStyle());
                }

                if (element != null)
                {
					position = geometry.CenterPos;
					source.Add(element);   
                }
			}

            if (goToPosition)
            {
                map.SetFocusPos(position, 1.0f);
                map.SetZoom(16, 1.0f);
            }

            var popup = new BalloonPopup(position, builder.BuildStyle(), title, description);
            source.Add(popup);
		}
    }
}
