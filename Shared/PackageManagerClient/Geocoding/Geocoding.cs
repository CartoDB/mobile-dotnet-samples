
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Carto.Geocoding;

namespace Shared
{
    public class Geocoding : BasePackageManagerClient
	{
        public override string Source
        {
            get { return "geocoding:" + Sources.CartoVector; }
        }

        public const string PackageFolder = "com.carto.geocodingpackages";

        public GeocodingService Service { get; private set; }

		public bool IsInProgress { get; set; }

        public bool HasAddress
        {
            get { return Addresses.Count > 0; }
        }

        public Geocoding(string path) : base(path)
        {
            
        }

		public List<GeocodingResult> Addresses { get; private set; } = new List<GeocodingResult>();

        public void SetOnlineMode()
        {
            Service = new MapBoxOnlineGeocodingService(Tokens.MapBox);
        }

        public void SetOfflineMode()
        {
            Service = new PackageManagerGeocodingService(Manager);
        }

		public void MakeRequest(string text, Action complete)
		{
			if (IsInProgress)
			{
				return;
			}

			IsInProgress = true;

			Task.Run(delegate
			{
				var request = new GeocodingRequest(Projection, text);
                GeocodingResultVector results = new GeocodingResultVector();
                try
                {
                    results = Service.CalculateAddresses(request);
                }
                catch (Exception e)
                {
                    Carto.Utils.Log.Error("Geocoding failed: " + e.Message);
                }
                int count = results.Count;

				Addresses.Clear();

				for (int i = 0; i < count; i++)
				{
					GeocodingResult result = results[i];
					Addresses.Add(result);
				}

				IsInProgress = false;

				complete();
			});
		}

    }
}
