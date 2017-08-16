
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carto.Geocoding;
using Carto.PackageManager;

namespace Shared
{
    public class Geocoding : BasePackageManagerClient
	{
		public const string Source = "geocoding:carto.streets";

		public PackageManagerGeocodingService Service { get; private set; }

		public bool IsInProgress { get; set; }

        public bool HasAddress
        {
            get { return Addresses.Count > 0; }
        }

		public List<GeocodingResult> Addresses { get; private set; } = new List<GeocodingResult>();

		public Geocoding(string path)
		{
            string folder = CreateDirectory(path, "com.carto.geocodingpackages");

			Manager = new CartoPackageManager(Source, folder);

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
				GeocodingResultVector results = Service.CalculateAddresses(request);
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
