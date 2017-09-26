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
        // Should be overridden in child class
        public virtual string Source 
        { 
            get { throw new NotImplementedException(); } 
        }

        public CartoPackageManager Manager { get; set; }

        public PackageListener Listener { get; set; }

        public Projection Projection { get; set; }

        public bool IsManagerAttached
        {
            get { return Manager != null; }
        }

		public BasePackageManagerClient(string path)
		{
            Listener = new PackageListener();

            if (path == null)
            {
                // Path can be null when we don't want to use the package manager,
                // such as in Online Routing and Route Search,
                // but they still utilize a Routing.cs, that inherits from this class
                return;
            }

		    Manager = new CartoPackageManager(Source, path);
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

    }
}
