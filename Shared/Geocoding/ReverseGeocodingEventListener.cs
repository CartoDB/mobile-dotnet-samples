
using System;
using Carto.Core;
using Carto.Geocoding;
using Carto.Projections;
using Carto.Ui;

namespace Shared
{
    public class ReverseGeocodingEventListener : MapEventListener
	{
        public EventHandler<EventArgs> ResultFound;

        public ReverseGeocodingService Service { get; set; }

		Projection projection;

        public ReverseGeocodingEventListener(Projection projection)
        {
            this.projection = projection;
        }

        public override void OnMapClicked(MapClickInfo mapClickInfo)
        {
            MapPos position = mapClickInfo.ClickPos;
            var request = new ReverseGeocodingRequest(projection, position);

            var meters = 125.0f;
            request.SearchRadius = meters;

            GeocodingResultVector results = Service.CalculateAddresses(request);

            GeocodingResult result = null;

            int count = results.Count;

            // Scan the results list. If we found relatively close point-based match,
            // use this instead of the first result.
            // In case of POIs within buildings, this allows us to hightlight POI instead of the building

            if (count > 0)
            {
                result = results[0];
            }

            for (int i = 0; i < count; i++)
            {
                GeocodingResult other = results[i];

                // 0.8f means 125 * (1.0 - 0.9) = 12.5 meters (rank is relative distance)
                if (other.Rank > 0.9f)
                {
                    string name = other.Address.Name;
					// Points of interest usually have names, others just have addresses
					if (!string.IsNullOrWhiteSpace(name))
                    {
                        result = other;
                        break;
                    }
                }
			}

            ResultFound?.Invoke(result, EventArgs.Empty);
		}
    }
}
