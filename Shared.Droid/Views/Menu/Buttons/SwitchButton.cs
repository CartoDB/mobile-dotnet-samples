using System;
using Android.Content;

namespace Shared.Droid
{
    public class SwitchButton : ActionButton
    {
        int resource1, resource2;

        public bool IsOn { get { return (int)imageView.Tag == resource1; } }

        public SwitchButton(Context context, int resource1, int resource2) : base(context, resource1)
        {
            this.resource1 = resource1;
            this.resource2 = resource2;
        }

        public override bool OnTouchEvent(Android.Views.MotionEvent e)
        {
            if (isEnabled)
            {
                if (e.Action == Android.Views.MotionEventActions.Up)
                {
                    Toggle();
                }
            }

            return base.OnTouchEvent(e);
        }

        void Toggle()
        {
            if (IsOn)
            {
                SetImageResource(resource2);
            } 
            else
            {
                SetImageResource(resource1);
            }
        }

        public void SetOff()
        {
            SetImageResource(resource2);
        }

    }
}
