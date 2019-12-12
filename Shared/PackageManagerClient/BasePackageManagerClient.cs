using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Carto.PackageManager;
using Carto.Projections;
using Shared.Model;

namespace Shared
{
    public class BasePackageManagerClient
    {
        private String _path;
        private CartoPackageManager _manager;
        private PackageListener _listener;

        // Should be overridden in child class
        public virtual string Source 
        { 
            get { throw new NotImplementedException(); } 
        }

        public CartoPackageManager Manager
        {
            get
            {
                if (_path != null && _manager == null)
                {
                    _manager = new CartoPackageManager(Source, _path);
                }
                return _manager;
            }
        }

        public bool IsManagerAttached
        {
            get { return Manager != null; }
        }

        public PackageListener Listener
        {
            get
            {
                if (_listener == null)
                {
                    _listener = new PackageListener();
                }
                return _listener;
            }
        }

        public Projection Projection { get; set; }

		public BasePackageManagerClient(string path)
		{
            _path = path;
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
            List<Package> packages = new List<Package>();

            if (folder.Equals(Package.CUSTOM_REGION_FOLDER_NAME + "/"))
            {
				return GetCustomRegionPackages();
            }

            if (folder.Equals(""))
            {
                packages.Add(GetCustomRegionFolder());    
            }

            foreach (PackageInfo info in Manager.ServerPackages)
            {
                string name = info.Name;
                if (!name.StartsWith(folder, StringComparison.Ordinal))
                {
                    // Belongs to a different folder,
                    // should not be added if name is e.g. Asia/, while folder is /Europe
                    continue;
                }

                string modified = name.Substring(folder.Length);
                int index = modified.IndexOf('/');
                Package package;

                if (index == -1)
                {
                    // This is an actual package
                    PackageStatus status = Manager.GetLocalPackageStatus(info.PackageId, -1);
                    string id = info.PackageId;
                    package = new Package(id, modified, info, status);
                }
                else
                {
                    // This is a package group
                    modified = modified.Substring(0, index);

                    // Try n' find an existing package from the list.
                    List<Package> existing = packages.Where(i => i.Name == modified).ToList();

                    if (existing.Count == 0)
                    {
                        // If there are none, add a package group if we don't have an existing list item
                        package = new Package(modified);
                    }
                    else if (existing.Count == 1 && existing[0].Info != null)
                    {
                        // Sometimes we need to add two labels with the same name.
                        // One a downloadable package and the other pointing to a list of said country's counties,
                        // such as with Spain, Germany, France, Great Britain

                        // If there is one existing package and its info isn't null,
                        // we will add a "parent" package containing subpackages (or package group)
                        package = new Package(modified);
                    }
                    else
                    {
                        // Shouldn't be added, as both cases are accounted for
                        continue;
                    }

                }

                packages.Add(package);

            }

            return packages;
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

                var local = GetAllPackages().Where(p => p.IsDownloading || p.IsQueued).ToList();

                foreach (Package element in local)
                {
                    bool found = false;
					foreach (Package existing in downloadQueue)
					{
                        if (existing.Id == element.Id)
                        {
                            existing.UpdateStatus(element.Status);
                            existing.UpdateInfo(element.Info);
                            found = true;
                        }
					}

                    if (!found)
                    {
                        downloadQueue.Add(element);
                    }
				}

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

                var package = new Package(id, modified, info, status);

                packages.Add(package);
            }

            packages.AddRange(GetCustomRegionPackages());

            return packages;
        }

        Package GetCustomRegionFolder()
        {
            return new Package(Package.CUSTOM_REGION_FOLDER_NAME, "NONE");
        }

        List<Package> GetCustomRegionPackages()
        {
            var packages = new List<Package>();

            foreach (City city in Cities.List)
            {
                var id = city.BoundingBox.ToString();
                var status = Manager.GetLocalPackageStatus(id, -1);

                var package = new Package(city.Name, id);
                package.UpdateStatus(status);

                packages.Add(package);
            }

            return packages;
        }
    }
}
