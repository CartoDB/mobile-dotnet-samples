using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
    public class BaseView : RelativeLayout
    {
        CGRect frame = CGRect.Empty;
        public CGRect Frame 
        {
            get { return frame; }
            set { 
                frame = value;

				var parameters = new RelativeLayout.LayoutParams(frame.W, frame.H);
                parameters.LeftMargin = frame.X;
                parameters.TopMargin = frame.Y;

				LayoutParameters = parameters;

                LayoutSubviews();
            }
        }

        public float Density 
        { 
            get { return Context.Resources.DisplayMetrics.Density; } 
        }

        public int UsableHeight
        {
            get
            {
                int total = Resources.DisplayMetrics.HeightPixels;
                return total - (NavigationBarHeight + StatusBarHeight /*+ ActionBarHeight*/);
            }
        }

        public int NavigationBarHeight { get { return GetSize("navigation_bar_height"); } }

		public int StatusBarHeight { get { return GetSize("status_bar_height"); } }

        int GetSize(string of)
        {
            var result = 0;
            var resourceId = Resources.GetIdentifier(of, "dimen", "android");

            if (resourceId > 0)
            {
                result = Resources.GetDimensionPixelSize(resourceId);    
            }

            return result;
        }

		public int ActionBarHeight
		{
			get
			{
                var tv = new TypedValue();
                var id = Android.Resource.Attribute.ActionBarSize;
                Context.Theme.ResolveAttribute(id, tv, true);
                return Resources.GetDimensionPixelSize(tv.ResourceId);
			}
		}

        public DisplayMetrics Metrics
        {
            get { return Context.Resources.DisplayMetrics; }
        }

        public bool IsLandscape
        {
            get { return Metrics.WidthPixels > Metrics.HeightPixels; }
        }

        public bool IsLargeTablet
        {
            get
            {
                var width = Metrics.WidthPixels;
                var height = Metrics.HeightPixels;

                var greater = height;
                var lesser = width;

                if (IsLandscape)
                {
                    greater = width;
                    lesser = height;
                }

                if (Density > 2.5f)
                {
                    // If density is too large, it'll be a phone
                    return false;
                }

                return greater > 1920 && lesser > 1080;
            }
        }

		public BaseView(Context context) : base(context) { }

        public virtual void LayoutSubviews() { }
    }
}
