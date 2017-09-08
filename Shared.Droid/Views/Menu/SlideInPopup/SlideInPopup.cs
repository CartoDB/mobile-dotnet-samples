
using System;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.OS;

namespace Shared.Droid
{
    public class SlideInPopup : BaseView
    {
        BaseView transparentArea;
        PopupView popup;

        int hiddenY, visibleY = -1;

        BaseView content;

        public PopupHeader Header { get { return popup.Header; } }

        public void ShowBackButton()
        {
            Header.BackButton.Visibility = Android.Views.ViewStates.Visible;
        }

		public void HideBackButton()
		{
            Header.BackButton.Visibility = Android.Views.ViewStates.Gone;
		}

        public SlideInPopup(Context context, int backIcon, int closeIcon) : base(context)
        {
            transparentArea = new BaseView(context);
            transparentArea.SetBackgroundColor(Color.Black);
            transparentArea.Alpha = 0.0f;
            AddView(transparentArea);

            popup = new PopupView(context, backIcon, closeIcon);
            AddView(popup);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Elevation = 11.0f;
            }
        }

        public override void LayoutSubviews()
        {
            int x = 0;
            int y = 0;
            int w = Frame.W;
            int h = Frame.H;

            transparentArea.SetFrame(x, y, w, h);

            hiddenY = h;
            visibleY = h - (h / 5 * 3);

            if (IsLandscape || IsLargeTablet)
            {
                w = (int)(400 * Density);
                visibleY = 0;
            }

            if (!IsLandscape && IsLargeTablet)
            {
                h = Frame.W;
                visibleY = Frame.H - h;
            }

            y = visibleY;
            popup.Frame = new CGRect(x, y, w, h);

            Hide(0);
        }

        public void SetPopupContent(BaseView content)
        {
            if (this.content != null)
            {
                popup.RemoveView(this.content);
                this.content = null;
            }

            this.content = content;
            popup.AddView(content);

            int x = 0;
            int y = popup.Header.TotalHeight;
            int w = popup.Frame.W;
            int h = popup.Frame.H - popup.Header.TotalHeight;

            content.SetFrame(x, y, w, h);
        }

        public void Show()
        {
            BringToFront();
            Visibility = Android.Views.ViewStates.Visible;

            AnimateAlpha(0.5f);
            AnimateY(visibleY);

            transparentArea.Click += Hide;
            popup.Header.CloseButton.Click += Hide;
        }

        public void Hide(long duration = 200)
        {
            AnimateAlpha(0.0f, duration);
            AnimateY(hiddenY, duration);

			transparentArea.Click -= Hide;
			popup.Header.CloseButton.Click -= Hide;
        }

        void Hide(object sender, EventArgs e)
        {
            Hide();    
        }

        void AnimateAlpha(float to, long duration = 200)
        {
            var animator = ObjectAnimator.OfFloat(transparentArea, "Alpha", to);
            animator.SetDuration(duration);
            animator.Start();
        }

        void AnimateY(int to, long duration = 200)
        {
            var animator = ObjectAnimator.OfFloat(popup, "y", to);
            animator.SetDuration(duration);
            animator.Start();

            animator.AnimationEnd += (object sender, EventArgs e) => {
                if (to == hiddenY)
                {
                    Visibility = Android.Views.ViewStates.Gone;
                }

                animator.Dispose();
            };
        }

    }
}
