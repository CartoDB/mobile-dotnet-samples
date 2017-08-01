
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<Package> GetPackages(string folder)
        {
            var language = "";
            return Manager.GetPackages(language, folder);
        }

        public void HandlePackageStatusChange(Package package)
        {
			string action = package.ActionText;
			string id = package.Id;

			switch (action)
			{
				case Package.ACTION_DOWNLOAD:
					Manager.StartPackageDownload(id);
					Enqueue(package);
					break;
				case Package.ACTION_PAUSE:
					Manager.SetPackagePriority(id, -1);
					Dequeue(package);
					break;
				case Package.ACTION_RESUME:
					Manager.SetPackagePriority(id, 0);
					Enqueue(package);
					break;
				case Package.ACTION_CANCEL:
					Manager.CancelPackageTasks(id);
					Dequeue(package);
					break;
				case Package.ACTION_REMOVE:
					Manager.StartPackageRemove(id);
					Dequeue(package);
					break;
			}
        }

        void Enqueue(Package package)
        {
            downloadQueue.Add(package);
        }

        void Dequeue(Package package)
        {
            downloadQueue.Remove(package);    
        }

        readonly List<Package> downloadQueue = new List<Package>();

        public Package CurrentDownload
        {
            get
            {
                if (downloadQueue.Count > 0) {
                    var downloading = downloadQueue.FirstOrDefault(p => p.IsDownloading);
                    if (downloading != null)
                    {
                        return downloading;
                    }
                }

                downloadQueue.Clear();
                downloadQueue.AddRange(GetAllPackages());

                if (downloadQueue.Count > 0)
                {
					var downloading = downloadQueue.FirstOrDefault(p => p.IsDownloading);
					if (downloading != null)
					{
						return downloading;
					}
                }

                return null;
            }
        }

        public List<Package> GetAllPackages()
        {
            var packages = new List<Package>();

            PackageInfoVector vector = Manager.ServerPackages;
            int total = vector.Count;

            for (int i = 0; i < total; i++)
            {
                PackageInfo info = vector[i];
                string name = info.Name;

                string[] split = name.Split('/');

                if (split.Length == 0)
                {
                    continue;
                }

                string modified = split[split.Length - 1];

                string id = info.PackageId;
                PackageStatus status = Manager.GetLocalPackageStatus(id, -1);
                var package = new Package(modified, info, status);

                packages.Add(package);
            }

            return packages;
        }
    }
}
