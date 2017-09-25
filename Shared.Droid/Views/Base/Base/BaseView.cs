using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
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

        public void SetInternalFrame(int x, int y, int width, int height)
        {
            frame = new CGRect(x, y, width, height);
        }

        public float Density 
        { 
            get { return Context.Resources.DisplayMetrics.Density; } 
        }

        public Rect HitRect
        {
            get
            {
                var rect = new Rect();
                GetHitRect(rect);
                return rect;
            }
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

        public bool IsJellybeanOrHigher
        {
            get { return Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean; }
        }

        public override void SetBackgroundColor(Android.Graphics.Color color)
        {
            var drawable = new GradientDrawable();
            drawable.SetColor(color);
            Background = drawable;
        }

        public void SetBorderColor(int width, Color color)
        {
            if (Background is GradientDrawable) {
                (Background as GradientDrawable).SetStroke(width, color);
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

        public void CloseKeyboard()
        {
            if (!(Context is Activity))
            {
                return;   
            }

            View view = (Context as Activity).CurrentFocus;

            if (view != null)
            {
                var service = Context.GetSystemService(Context.InputMethodService);
                var manager = service as InputMethodManager;
                manager.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }
    }
}
