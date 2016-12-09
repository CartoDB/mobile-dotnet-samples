using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.PackageManager;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class CountryChoiceMenu : BaseMenu
	{
		ListView list;
		PackageManagerAdapter adapter;
		List<Package> packages = new List<Package>();

		public CountryChoiceMenu(Context context) : base(context)
		{
			list = new ListView(context);
			list.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			AddView(list);

			adapter = new PackageManagerAdapter(context, list, Resource.Layout.package_item_row, packages);
			list.Adapter = adapter;
		}

		public void Update(List<Package> packages)
		{
			this.packages.Clear();
			this.packages.AddRange(packages);

			adapter.NotifyDataSetChanged();
		}

		public void UpdateWithRoutingPackages(List<Package> packages)
		{
			Console.WriteLine("UpdateWithRoutingPackages: start");
			this.packages.Clear();
			this.packages.AddRange(packages);

			int counter = 0;

			foreach (Package package in this.packages)
			{
				Package routing = packages.Find(pkg => pkg.RoutingId == package.Id);

				Console.WriteLine("Upading..." + counter);
				counter++;

				if (routing != null)
				{
					package.UpdateStatus(routing.Status);
				}
			}
			Console.WriteLine("UpdateWithRoutingPackages: end");
			adapter.NotifyDataSetChanged();
		}

		public void Update(PackageManager manager, string id)
		{
			// Try to find the package that needs to be updated
			for (int i = 0; i < packages.Count; i++)
			{
				Package pkg = packages[i];

				if (id.Equals(pkg.Id))
				{
					PackageStatus status = manager.GetLocalPackageStatus(id, -1);
					pkg.UpdateStatus(status);

					packages[i] = pkg;

					pkg.ToMapPackage();

					adapter.Update(pkg);
				}
			}
		}
	}
}
