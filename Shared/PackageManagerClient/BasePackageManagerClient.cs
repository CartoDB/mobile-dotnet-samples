using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carto.PackageManager;
using Carto.Projections;

namespace Shared
{
    public class BasePackageManagerClient
    {
        public CartoPackageManager Manager { get; set; }

        public PackageListener Listener { get; set; }

        public Projection Projection { get; set; }

        public BasePackageManagerClient()
        {
            Listener = new PackageListener();
        }

        public void AttachListener()
        {
            Manager.PackageManagerListener = Listener;
        }

        public void RemoveListener()
        {
            Manager.PackageManagerListener = null;
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
                if (downloadQueue.Count > 0)
                {
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

        public string CreateDirectory(string path, string folder)
        {
#if __ANDROID__
            var directory = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory, folder);

            if (!directory.Exists())
            {
                directory.Mkdir();
            }

            return directory.AbsolutePath;
#elif __IOS__
            string directory = Path.Combine(path, folder);

			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

            return directory;
#endif
		}
    }
}
