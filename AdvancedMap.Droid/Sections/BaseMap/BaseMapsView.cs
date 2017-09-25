using System;
using Android.Content;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Views
{
	public class BaseMapsView : MapBaseView
	{
		public BaseMapsView(Context context) : base(context,
													Resource.Drawable.icon_info_blue,
													Resource.Drawable.icon_back_blue,
												    Resource.Drawable.icon_close)
		{
		}
	}
}
