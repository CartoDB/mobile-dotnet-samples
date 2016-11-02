using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.PackageManager;
using Shared;

namespace AdvancedMap.Droid
{
	public class PackageManagerAdapter : ArrayAdapter<Package>
	{
		Context context;
		int layoutResourceId;
		List<Package> packages;

		public override int Count
		{
			get
			{
				return packages.Count;
			}
		}

		PackageManagerActivity Activity { get { return Context as PackageManagerActivity; } }

		public PackageManagerAdapter(Context context, int resId, List<Package> packages) : base(context, resId, packages)
		{
			this.context = context;
			this.layoutResourceId = resId;
			this.packages = packages;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View row = convertView;
			PackageHolder holder = null;

			// Create new holder object or reuse existing
			if (row == null)
			{
				LayoutInflater inflater = ((Activity)context).LayoutInflater;
				row = inflater.Inflate(layoutResourceId, parent, false);

				holder = new PackageHolder();
				holder.NameView = (TextView)row.FindViewById(Resource.Id.package_name);
				holder.StatusView = (TextView)row.FindViewById(Resource.Id.package_status);
				holder.ActionButton = (PackageManagerButton)row.FindViewById(Resource.Id.package_action);

				row.Tag = holder;
			}
			else {
				holder = (PackageHolder)row.Tag;
			}

			// Report package name and size
			Package pkg = packages[position];
			holder.NameView.Text = pkg.Name;

			if (pkg.Info != null)
			{
				string status = "available";

				if (pkg.IsSmallerThan1MB)
				{
					status += " v." + pkg.Info.Version + " (<1MB)";
				}
				else {
					status += " v." + pkg.Info.Version + " (" + pkg.Info.Size.ToLong() / 1024 / 1024 + "MB)";
				}

				holder.ActionButton.PackageId = pkg.Info.PackageId;

				// Check if the package is downloaded/is being downloaded (so that status is not null)
				if (pkg.Status != null)
				{
					if (pkg.Status.CurrentAction == PackageAction.PackageActionReady)
					{
						status = "ready";
						holder.ActionButton.Text = "RM";
						holder.ActionButton.Type = PMButtonType.StartRemovePackage;
					}
					else if (pkg.Status.CurrentAction == PackageAction.PackageActionWaiting)
					{
						status = "queued";
						holder.ActionButton.Text = "C";
						holder.ActionButton.Type = PMButtonType.CancelPackageTasks;
					}
					else {
						if (pkg.Status.CurrentAction == PackageAction.PackageActionCopying)
						{
							status = "copying";
						}
						else if (pkg.Status.CurrentAction == PackageAction.PackageActionDownloading)
						{
							status = "downloading";
						}
						else if (pkg.Status.CurrentAction == PackageAction.PackageActionRemoving)
						{
							status = "removing";
						}

						status += " " + ((int)pkg.Status.Progress).ToString() + "%";

						if (pkg.Status.Paused)
						{
							status = status + " (paused)";
							holder.ActionButton.Text = "R";
							holder.ActionButton.Type = PMButtonType.SetPackagePriority;
							holder.ActionButton.PriorityIndex = 0;
						}
						else {
							holder.ActionButton.Text = "P";
							holder.ActionButton.Type = PMButtonType.SetPackagePriority;
							holder.ActionButton.PriorityIndex = -1;
						}
					}
				}
				else {
					holder.ActionButton.Text = "DL";
					holder.ActionButton.Type = PMButtonType.StartPackageDownload;
				}

				holder.StatusView.Text = status;
			}
			else {
				holder.ActionButton.Text = ">";
				holder.ActionButton.Type = PMButtonType.UpdatePackages;
				holder.ActionButton.PackageName = pkg.Name;
				holder.StatusView.Text = "";
			}

			// Always Detach handler first to avoid multiple handlers on reuse
			holder.ActionButton.Click -= Activity.OnAdapterActionButtonClick;
			holder.ActionButton.Click += Activity.OnAdapterActionButtonClick;

			return row;
		}
	}

}

