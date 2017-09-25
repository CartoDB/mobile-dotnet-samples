using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Shared.Droid;
using Shared.Model;

namespace AdvancedMap.Droid.Sections.BaseMap.Subviews
{
    public class LanguageCell : BaseView
    {
        TextView label;

        public Language Item { get; private set; }

        public LanguageCell(Context context) : base(context)
        {
            label = new TextView(context);
            label.TextSize = 15.0f;
            label.Gravity = Android.Views.GravityFlags.CenterVertical;
            label.Typeface = Typeface.DefaultBold;
            AddView(label);
        }

        public override void LayoutSubviews()
        {
            int padding = (int)(15 * Density);
            label.SetFrame(padding, 0, Frame.W - padding, Frame.H);
        }

        public void Update(Language language)
        {
            Item = language;
            label.Text = language.Name.ToUpper();
        }
    }
}
