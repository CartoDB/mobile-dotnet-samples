using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.PackageManager;

namespace CartoMobileSample
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
				holder.nameView = (TextView)row.FindViewById(HelloMap.Resource.Id.package_name);
				holder.statusView = (TextView)row.FindViewById(HelloMap.Resource.Id.package_status);
				holder.actionButton = (Button)row.FindViewById(HelloMap.Resource.Id.package_action);

				row.Tag = holder;
			}
			else {
				holder = (PackageHolder)row.Tag;
			}

			// Report package name and size
			Package pkg = packages[position];
			holder.nameView.Text = pkg.Name;

			if (pkg.Info != null)
			{
				String status = "available";
				if (pkg.Info.Size.ToLong() < 1024 * 1024)
				{
					status += " v." + pkg.Info.Version + " (<1MB)";
				}
				else {
					status += " v." + pkg.Info.Version + " (" + pkg.Info.Size.ToLong() / 1024 / 1024 + "MB)";
				}

				// Check if the package is downloaded/is being downloaded (so that status is not null)
				if (pkg.Status != null)
				{
					if (pkg.Status.CurrentAction == PackageAction.PackageActionReady)
					{
						status = "ready";
						holder.actionButton.Text = "RM";
						//holder.actionButton.setOnClickListener(new OnClickListener()
						//{
						//	public override void OnClick(View v)
						//	{
						//		packageManager.startPackageRemove(pkg.packageInfo.getPackageId());
						//	}
						//});
					}
					else if (pkg.Status.CurrentAction == PackageAction.PackageActionWaiting)
					{
						status = "queued";
						holder.actionButton.Text = "C";
						//holder.actionButton.setOnClickListener(new OnClickListener()
						//{
						//	public override void OnClick(View v)
						//	{
						//		packageManager.cancelPackageTasks(pkg.packageInfo.getPackageId());
						//	}
						//});
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
							holder.actionButton.Text = "R";
							//holder.actionButton.setOnClickListener(new OnClickListener()
							//{
							//	public override void OnClick(View v)
							//	{
							//		packageManager.setPackagePriority(pkg.packageInfo.getPackageId(), 0);
							//	}
							//});
						}
						else {
							holder.actionButton.Text = "P";
							//holder.actionButton.setOnClickListener(new OnClickListener()
							//{
							//	public override void OnClick(View v)
							//	{
							//		packageManager.setPackagePriority(pkg.packageInfo.getPackageId(), -1);
							//	}
							//});
						}
					}
				}
				else {
					holder.actionButton.Text = "DL";
					//holder.actionButton.setOnClickListener(new OnClickListener()
					//{
					//	public override void OnClick(View v)
					//	{
					//		packageManager.startPackageDownload(pkg.packageInfo.getPackageId());
					//	}
					//});
				}
				holder.statusView.Text = status;
			}
			else {
				holder.actionButton.Text = ">";
				//holder.actionButton.setOnClickListener(new OnClickListener()
				//{
				//	public override void OnClick(View v)
				//	{
				//		currentFolder = currentFolder + pkg.packageName + "/";
				//		updatePackages();
				//	}
				//});
				holder.statusView.Text = "";
			}

			return row;
		}
	}

}

