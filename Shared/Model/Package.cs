
using System;
using Carto.PackageManager;

namespace Shared
{
	/**
	* Full package info
	*/
	public class Package
	{
		public const string ROUTING = "";

		public string Name { get; private set; }
		public string Id { get; private set; }

		public PackageInfo Info { get; private set; }
		public PackageStatus Status { get; private set; }

		public bool IsSmallerThan1MB { get { return SizeInMb < 1; } }
        public double SizeInMb { get { return Info.Size.ToLong() / (1024 * 1024); } }
		
        public bool HasInfo { get { return Info != null; } }

        public bool IsGroup
        {
            get { return Status == null && Info == null && !IsCustomRegionPackage; }
        }

        public Package(string name)
        {
            Name = name;
        }

        public Package(string name, string id)
        {
            Name = name;
            Id = id;
        }

		public Package(string id, string name, PackageInfo info, PackageStatus status)
		{
            Id = id;
			Name = name;
			Info = info;
			Status = status;
		}

        public const string BBOX_IDENTIFIER = "bbox(";
        public bool IsCustomRegionPackage
        {
            get
            {
				if (Id == null)
				{
					return false;
				}

                return Id.Contains(BBOX_IDENTIFIER);
			}
        }

        public const string CUSTOM_REGION_FOLDER_NAME = "CUSTOM REGIONS";
        public bool IsCustomRegionFolder
        {
            get { return Name.Equals(CUSTOM_REGION_FOLDER_NAME); }
        }

		public void UpdateStatus(PackageStatus status)
		{
			Status = status;
		}

        public void UpdateInfo(PackageInfo info)
        {
            Info = info;
        }

		public void ToMapPackage()
		{
			Id = Id.Replace(ROUTING, "");
		}

		public string GetStatusText()
		{
			if (Info == null)
			{
				return "";
			}

			string status = "available";

            status += GetVersionAndSize();

			// Check if the package is downloaded/is being downloaded (so that status is not null)
			if (Status != null)
			{
				if (Status.CurrentAction == PackageAction.PackageActionReady)
				{
					status = "ready";
				}
				else if (Status.CurrentAction == PackageAction.PackageActionWaiting)
				{
					status = "queued";
				}
				else {
					if (Status.CurrentAction == PackageAction.PackageActionCopying)
					{
						status = "copying";
					}
					else if (Status.CurrentAction == PackageAction.PackageActionDownloading)
					{
						status = "downloading";
					}
					else if (Status.CurrentAction == PackageAction.PackageActionRemoving)
					{
						status = "removing";
					}

					status += " " + ((int)Status.Progress).ToString() + "%";
				}
			}

			return status;
		}

        string GetVersionAndSize()
        {
            if (IsCustomRegionPackage)
            {
                return "";
            }

            string version = Info.Version.ToString();
            string size = SizeInMb.ToString("F");

            if (IsSmallerThan1MB)
            {
				return " v." + version + " (<1MB)";
			}

            return " v." + version + " (" + size + " MB)";
        }

        public const string ACTION_DOWNLOAD = "DOWNLOAD";
        public const string ACTION_REMOVE = "REMOVE";
        public const string ACTION_CANCEL = "CANCEL";
        public const string ACTION_RESUME = "RESUME";
        public const string ACTION_PAUSE = "PAUSE";

        public string ActionText
        {
            get
            {
                if (Status == null)
                {
                    return ACTION_DOWNLOAD;
                }

                if (Status.CurrentAction == PackageAction.PackageActionReady)
                {
                    return ACTION_REMOVE;
                } 

                if (Status.CurrentAction == PackageAction.PackageActionWaiting)
                {
                    return ACTION_CANCEL;
                }

                if (Status.Paused)
                {
                    return ACTION_RESUME;
                }

                return ACTION_PAUSE;
            }
        }

        public bool IsDownloading
        {
            get
            {
                if (Status == null)
                {
                    return false;
                }

                PackageAction action = Status.CurrentAction;
                return action == PackageAction.PackageActionDownloading && !Status.Paused;
            }
        }

		public bool IsQueued
		{
			get
			{
				if (Status == null)
				{
					return false;
				}

				PackageAction action = Status.CurrentAction;
                return action == PackageAction.PackageActionWaiting && !Status.Paused;
			}
		}

	}
}

