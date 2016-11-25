using System;
using Android.Content;
using Android.Views;
using Carto.DataSources;
using Carto.PackageManager;

namespace AdvancedMap.Droid
{
	public class MenuClickListener : Java.Lang.Object, IMenuItemOnMenuItemClickListener
	{
		public static CartoPackageManager Manager;

		AdvancedPackageManagerActivity context;

		public MenuClickListener(AdvancedPackageManagerActivity context)
		{
			this.context = context;
		}

		public bool OnMenuItemClick(IMenuItem item)
		{
			// Using static global variable to pass data. Avoid this in your app (memory leaks etc)!
			Manager = context.packageManager;

			Intent intent = new Intent(context, typeof(PackagedMapActivity));
			context.StartActivity(intent);

			return true;
		}
	}

}
