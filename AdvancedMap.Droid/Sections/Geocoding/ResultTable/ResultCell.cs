using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Carto.Geocoding;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class ResultTableCell : BaseView
    {
        TextView label;

        public ResultTableCell(Context context) : base(context)
        {
            label = new TextView(context);
            label.TextSize = 15.0f;
            label.Gravity = Android.Views.GravityFlags.CenterVertical;
            label.SetTextColor(Color.White);

            AddView(label);
        }

        public override void LayoutSubviews()
        {
            int leftPadding = (int)(5 * Density);
            label.SetFrame(leftPadding, 0, Frame.W - 2 * leftPadding, Frame.H);
        }

        public void Update(GeocodingResult item)
        {
            label.Text = item.GetPrettyAddress();
            LayoutSubviews();
        }
    }
}
