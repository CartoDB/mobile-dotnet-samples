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
				holder.Button = (PackageManagerButton)row.FindViewById(Resource.Id.package_action);

				row.Tag = holder;
			}
			else {
				holder = (PackageHolder)row.Tag;
			}

			Package package = packages[position];

			holder.NameView.Text = package.Name;
			// Parse status and button texts
			holder.StatusView.Text = package.GetStatusText();

			ButtonInfo info = package.GetButtonInfo();
			holder.Button.Text = info.Text;
			holder.Button.Type = info.Type;
			holder.Button.PriorityIndex = info.PriorityIndex;
			holder.Button.PackageName = info.PackageName;
			holder.Button.PackageId = info.PackageId;

			// Always Detach handler first to avoid multiple handlers on reuse
			holder.Button.Click -= Activity.OnAdapterActionButtonClick;
			holder.Button.Click += Activity.OnAdapterActionButtonClick;

			return row;
		}
	}

}

