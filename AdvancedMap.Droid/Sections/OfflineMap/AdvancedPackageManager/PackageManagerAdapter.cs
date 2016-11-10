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
		List<Package> packages;

		public override int Count { get { return packages.Count; } }

		AdvancedPackageManagerActivity Activity { get { return Context as AdvancedPackageManagerActivity; } }

		public PackageManagerAdapter(Context context, int resId, List<Package> packages) : base(context, resId, packages)
		{
			this.context = context;
			this.packages = packages;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			PackageRow row = (PackageRow)convertView;
			Package package = packages[position];

			if (row == null)
			{
				row = new PackageRow(context);
			}

			// Always Detach handler first to avoid multiple handlers on reuse
			row.Button.Click -= Activity.OnAdapterActionButtonClick;
			row.Button.Click += Activity.OnAdapterActionButtonClick;

			row.Update(package);

			return row;
		}
	}

}

