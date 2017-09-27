
using Android.Content;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.OfflineMap
{
    public class OfflineMapView : PackageDownloadBaseView
	{
		public OfflineMapView(Context context) : base(context,
														 Resource.Drawable.icon_info_blue,
														 Resource.Drawable.icon_back_blue,
														 Resource.Drawable.icon_close,
														 Resource.Drawable.icon_global,
														 Resource.Drawable.icon_wifi_on,
														 Resource.Drawable.icon_wifi_off,
														 Resource.Drawable.icon_forward_blue,
                                                         Resource.Drawable.icon_info_white
														)
		{
            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
		}

	}
}
