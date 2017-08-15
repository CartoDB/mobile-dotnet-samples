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
                return total - (NavigationBarHeight + StatusBarHeight + ActionBarHeight);
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

		public BaseView(Context context) : base(context) { }

        public virtual void LayoutSubviews() { }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return true;
        }
    }
}
