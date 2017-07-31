
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Carto.Geocoding;
using Carto.PackageManager;
using Carto.Projections;

namespace Shared
{
	public class Geocoding
	{
		public const string Source = "geocoding:carto.streets";

		public PackageManagerGeocodingService Service { get; private set; }

		public CartoPackageManager Manager { get; private set; }

		public Projection Projection { get; set; }

		public bool IsInProgress { get; set; }
		public List<GeocodingResult> Addresses { get; private set; } = new List<GeocodingResult>();

		public PackageListener Listener { get; private set; }

		public Geocoding(string path)
		{
            string folder = Path.Combine(path, "geocodingpackages");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			Manager = new CartoPackageManager(Source, folder);

			Service = new PackageManagerGeocodingService(Manager);

			Listener = new PackageListener();
			Manager.PackageManagerListener = Listener;
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
