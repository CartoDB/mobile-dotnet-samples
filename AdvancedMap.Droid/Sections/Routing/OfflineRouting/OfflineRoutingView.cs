using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Carto.PackageManager;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class OfflineRoutingView : MapWithMenuButton
	{
		public CountryChoiceMenu Menu { get; set; }

		public OfflineRoutingView(Context context) : base(context, Resource.Drawable.icon_menu_round)
		{
			Menu = new CountryChoiceMenu(context);
			AddView(Menu);
		}

		public void UpdateList(List<Package> packages)
		{
			Menu.Update(packages);
		}

		public void UpdatePackage(PackageManager manager, string id)
		{
			Menu.Update(manager, id);
		}
}
}
