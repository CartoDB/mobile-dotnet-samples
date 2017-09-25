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

		ListView list;

		public override int Count { get { return packages.Count; } }

		public PackageManagerAdapter(Context context, ListView list, int resId, List<Package> packages) : base(context, resId, packages)
		{
			this.list = list;

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

				if (Context is AdvancedPackageManagerActivity)
				{
					row.Button.Click += (context as AdvancedPackageManagerActivity).OnAdapterActionButtonClick;
				}
				else if (Context is OfflineRoutingActivity)
				{
					//row.Button.Click += (context as OfflineRoutingActivity).OnAdapterActionButtonClick;
				}
			}

			row.Update(package);

			return row;
		}

		public void Update(Package package)
		{
			for (int i = 0; i < list.ChildCount; i++)
			{
				PackageRow view = (PackageRow)list.GetChildAt(i);

				if (view.Id == package.Id)
				{
					view.Update(package);
				}
			}
		}
	}

}

