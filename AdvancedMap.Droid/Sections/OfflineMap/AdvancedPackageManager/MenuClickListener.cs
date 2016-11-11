using System;
using Android.Content;
using Android.Views;
using Carto.DataSources;

namespace AdvancedMap.Droid
{
	public class MenuClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		public static PackageManagerTileDataSource DataSource;

		AdvancedPackageManagerActivity context;

		public MenuClickListener(AdvancedPackageManagerActivity context)
		{
			this.context = context;
		}

		public bool OnMenuItemClick(IMenuItem item)
		{
			// Using static global variable to pass data. Avoid this in your app (memory leaks etc)!
			DataSource = new PackageManagerTileDataSource(context.packageManager);

			Intent intent = new Intent(context, typeof(PackagedMapActivity));
			context.StartActivity(intent);

			return true;
		}
	}

}
