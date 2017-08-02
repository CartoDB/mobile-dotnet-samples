
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

		public bool IsSmallerThan1MB { get { return Info.Size.ToLong() < 1024 * 1024; } }
		public bool HasInfo { get { return Info != null; } }

        public bool IsGroup
        {
            get { return Status == null && Info == null; }
        }

		public Package(string name, PackageInfo info, PackageStatus status)
		{
			Name = name;
			Id = (info != null ? info.PackageId : null);
			Info = info;
			Status = status;
		}

		public void UpdateStatus(PackageStatus status)
		{
			Status = status;
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

			if (IsSmallerThan1MB)
			{
				status += " v." + Info.Version + " (<1MB)";
			}
			else {
				status += " v." + Info.Version + " (" + Info.Size.ToLong() / 1024 / 1024 + "MB)";
			}

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

		public ButtonInfo GetButtonInfo()
		{
			if (Info == null)
			{
				return new ButtonInfo { Text = ">", Type = PMButtonType.UpdatePackages, PackageName = Name };
			}

			if (Status == null) 
			{
				return new ButtonInfo { Text = "Download", Type = PMButtonType.StartPackageDownload, PackageName = Name, PackageId = Info.PackageId };
			}

			ButtonInfo info = new ButtonInfo();
			info.PackageId = Info.PackageId;

			if (Status.CurrentAction == PackageAction.PackageActionReady)
			{
				info.Text = "Remove";
				info.Type = PMButtonType.StartRemovePackage;
			}
			else if (Status.CurrentAction == PackageAction.PackageActionWaiting)
			{
				info.Text = "Cancel";
				info.Type = PMButtonType.CancelPackageTasks;
			}
			else {

				if (Status.Paused)
				{
					info.Text = "Resume";
					info.Type = PMButtonType.SetPackagePriority;
					info.PriorityIndex = 0;
				}
				else {
					info.Text = "Pause";
					info.Type = PMButtonType.SetPackagePriority;
					info.PriorityIndex = -1;
				}
			}
			return info;
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
                if (Info == null)
                {
                    return "NONE";
                }

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

	public class ButtonInfo
	{
		public string Text { get; set; }

		public string PackageName { get; set; }

		public string PackageId { get; set; }

		public PMButtonType Type { get; set; }
		
		public int PriorityIndex { get; set; }
	}
}

