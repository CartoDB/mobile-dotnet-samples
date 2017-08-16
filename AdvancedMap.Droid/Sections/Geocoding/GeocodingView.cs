
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Carto.Geocoding;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
    public class GeocodingView : BaseGeocodingView
    {
		public EditText Field { get; private set; }

		public ListView ResultTable { get; private set; }

		public ResultTableAdapter Adapter { get; private set; }

		public GeocodingView(Context context) : base(context)
        {
            Field = new EditText(context);
            Field.SetTextColor(Color.White);
            Field.SetBackgroundColor(Colors.DarkTransparentNavy);
            AddView(Field);

            ResultTable = new ListView(context);
            ResultTable.SetBackgroundColor(Colors.LightTransparentNavy);
            AddView(ResultTable);

            Adapter = new ResultTableAdapter(context);
            ResultTable.Adapter = Adapter;

			Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
        }

		public override void LayoutSubviews()
		{
            base.LayoutSubviews();

			int padding = (int)(5 * Density);

			int x = padding;
			int y = padding;
			int w = Frame.W - 2 * padding;
			int h = (int)(45 * Density);

			Field.SetFrame(x, y, w, h);

			y += h + (int)(1 * Density);
			h = (int)(200 * Density);

			ResultTable.SetFrame(x, y, w, h);
			Adapter.Width = Frame.W;
		}

		public void UpdateList(List<GeocodingResult> results)
		{
			Adapter.Items.Clear();
			Adapter.Items.AddRange(results);
			Adapter.NotifyDataSetChanged();
		}

        public void ShowTable()
        {
            ResultTable.Visibility = Android.Views.ViewStates.Visible;
        }

        public void HideTable()
        {
            ResultTable.Visibility = Android.Views.ViewStates.Gone;
        }

        public void ClearInput()
        {
            Field.Text = "";
        }
    }
}
