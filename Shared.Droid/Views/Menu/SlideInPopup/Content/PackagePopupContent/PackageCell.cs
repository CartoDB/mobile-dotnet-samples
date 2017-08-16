
using System;
using Android.Content;
using Android.Widget;
using Android.Graphics;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class PackageCell : BaseView
    {
        public Package Package { get; private set; }

        TextView textLabel, title, subtitle, statusIndicator;
        ImageView forwardIcon;
        BaseView progressIndicator;

        public PackageCell(Context context, int icon_forward) : base(context)
        {
            float titleSize = 13.0f;
            Color titleColor = Colors.CartoNavy;

            textLabel = new TextView(context);
            textLabel.TextSize = titleSize;
            textLabel.SetTextColor(titleColor);
            textLabel.Gravity = Android.Views.GravityFlags.Center;
            textLabel.Typeface = Typeface.DefaultBold;
            AddView(textLabel);

            title = new TextView(context);
            title.TextSize = titleSize;
            title.SetTextColor(titleColor);
            title.Typeface = Typeface.DefaultBold;
            AddView(title);

            subtitle = new TextView(context);
            subtitle.TextSize = titleSize - 2;
            subtitle.SetTextColor(Color.LightGray);
            AddView(subtitle);

            statusIndicator = new TextView(context);
            statusIndicator.SetTextColor(Colors.AppleBlue);
            statusIndicator.Gravity = Android.Views.GravityFlags.Center;
            statusIndicator.TextSize = titleSize - 1;
            statusIndicator.Typeface = Typeface.DefaultBold;
            statusIndicator.SetBackground(Colors.AppleBlue);
            AddView(statusIndicator);

            forwardIcon = new ImageView(context);
            forwardIcon.SetImageResource(icon_forward);
            forwardIcon.SetScaleType(ImageView.ScaleType.CenterCrop);
            AddView(forwardIcon);

            progressIndicator = new BaseView(context);
            progressIndicator.SetBackground(Colors.AppleBlue);
            AddView(progressIndicator);
        }

        int LeftPadding { get { return (int)(15 * Density); } }

		public override void LayoutSubviews()
        {
            if (Package == null)
            {
                return;
            }

            int x, y, w, h;

            if (Package.IsGroup)
            {
                title.SetFrame(0, 0, 0, 0);
                subtitle.SetFrame(0, 0, 0, 0);
                statusIndicator.SetFrame(0, 0, 0, 0);
                progressIndicator.SetFrame(0, 0, 0, 0);

                h = Frame.H / 3;
                w = h / 2;
                x = Frame.W - (w + LeftPadding);
                y = Frame.H / 2 - h / 2;

                forwardIcon.SetFrame(x, y, w, h);
                textLabel.SetFrame(LeftPadding, 0, Frame.W, Frame.H);
                return;
            }

            title.Measure(0, 0);
            subtitle.Measure(0, 0);
            statusIndicator.Measure(0, 0);

            int topPadding = (Frame.H - (title.MeasuredHeight + subtitle.MeasuredHeight)) / 2;
            int titleWidth = (int)(Frame.W * 0.66);

            x = LeftPadding;
            y = topPadding;
            w = titleWidth;
            h = title.MeasuredHeight;

            title.SetFrame(x, y, w, h);

            y += h;

            subtitle.SetFrame(x, y, w, h);

            w = (int)(80 * Density);
            h = (int)(Frame.H / 5 * 3.1);
            x = Frame.W - (w + LeftPadding);
            y = Frame.H / 2 - h / 2;

            statusIndicator.SetFrame(x, y, w, h);
        }

        public void Update(Package package)
        {
            Package = package;

            if (package.IsGroup)
            {
                // It's a package group. These are displayed with a single label

                textLabel.Text = package.Name.ToUpper();
                forwardIcon.Visibility = Android.Views.ViewStates.Visible;
                return;
            }

            forwardIcon.Visibility = Android.Views.ViewStates.Gone;

            // "Hide" the original label, as these aren't used in advanced cells
            textLabel.Text = "";

            title.Text = package.Name.ToUpper();
            subtitle.Text = package.GetStatusText();

            string action = package.ActionText;
            statusIndicator.Text = action;

            int width = 0;

            if (action.Equals(Package.ACTION_DOWNLOAD))
            {
                width = (int)(1.2 * Density);
            }

            statusIndicator.SetCornerRadius(width);

            if (package.Status == null)
            {
                progressIndicator.Frame = CGRect.Empty;
                return;
            }

			if (package.Status.CurrentAction != PackageAction.PackageActionDownloading)
			{
				progressIndicator.Frame = CGRect.Empty;
                return;
			}

            UpdateProgress(package.Status.Progress);
        }

        public void Update(PackageStatus status)
        {
            Package.UpdateStatus(status);
            Update(Package);
        }

        public void Update(Package package, float progress)
        {
            Update(package);
            UpdateProgress(progress);
        }

        public void UpdateProgress(float progress)
        {
            int width = (int)((Frame.W - 2 * LeftPadding) * progress / 100);
            int height = (int)(1.5 * Density);
            int x = LeftPadding;
            int y = Frame.H - (int)(5 * Density);

            progressIndicator.Frame = new CGRect(x, y, width, height);
        }
    }
}
