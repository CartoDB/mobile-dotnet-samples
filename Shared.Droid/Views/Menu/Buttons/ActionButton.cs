using System;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
    public class ActionButton : BaseView
    {
        public EventHandler<EventArgs> Clicked;

        protected ImageView imageView;

        public ActionButton(Context context, int resource) : base(context)
        {
            if (this.IsLollipopOrHigher()) {
                Elevation = 5.0f;
            }

            this.SetBackground(Color.White);

            imageView = new ImageView(context);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            imageView.SetAdjustViewBounds(true);
            AddView(imageView);

            SetImageResource(resource);
        }

        public override void LayoutSubviews()
        {
            var padding = (int)(15 * Density);
            var size = Frame.H - 2 * padding;
            imageView.SetFrame(padding, padding, size, size);
			
            this.SetCornerRadius(Frame.W / 2);
		}

        protected bool isEnabled = true;
		
        public void Enable()
		{
			Alpha = 1.0f;
			isEnabled = false;
		}

        public void Disable()
        {
            Alpha = 0.5f;
            isEnabled = false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (!isEnabled)
            {
                return true;
            }

            if (e.Action == MotionEventActions.Down)
            {
                Alpha = 0.5f;
            }
            else if (e.Action == MotionEventActions.Cancel)
            {
                Alpha = 1.0f;
            }
            else if (e.Action == MotionEventActions.Up)
            {
                Alpha = 1.0f;
                Clicked?.Invoke(null, EventArgs.Empty);
            }

            return true;
        }

        protected void SetImageResource(int resource)
        {
            imageView.SetImageResource(resource);
            imageView.Tag = resource;
        }
    }
}
