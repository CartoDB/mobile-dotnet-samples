using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Carto.PackageManager;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class OfflineRoutingView : PackageDownloadBaseView
	{
		public OfflineRoutingView(Context context) : base(context,
														 Resource.Drawable.icon_info_blue,
														 Resource.Drawable.icon_back_blue,
														 Resource.Drawable.icon_close,
														 Resource.Drawable.icon_global,
														 Resource.Drawable.icon_wifi_on,
														 Resource.Drawable.icon_wifi_off,
														 Resource.Drawable.icon_forward_blue
														)
        {
            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
		}

	}
}
