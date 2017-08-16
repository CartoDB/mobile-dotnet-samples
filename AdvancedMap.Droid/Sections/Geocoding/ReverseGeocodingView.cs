using System;
using Android.Content;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class ReverseGeocodingView : BaseGeocodingView
    {
        public ReverseGeocodingView(Context context) : base(context)
        {
			Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
        }
    }
}
