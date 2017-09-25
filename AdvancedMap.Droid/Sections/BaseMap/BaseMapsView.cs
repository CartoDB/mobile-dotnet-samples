using System;
using AdvancedMap.Droid.Sections.BaseMap.Subviews;
using Android.Content;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Views
{
	public class BaseMapsView : MapBaseView
	{
        public ActionButton BasemapButton;
        public StylePopupContent StyleContent;

		public BaseMapsView(Context context) : base(context,
													Resource.Drawable.icon_info_blue,
													Resource.Drawable.icon_back_blue,
												    Resource.Drawable.icon_close)
		{
            BasemapButton = new ActionButton(context, Resource.Drawable.icon_basemap);
            AddButton(BasemapButton);

            StyleContent = new StylePopupContent(context);

            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
		}
	}
}
