using System;
using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Carto.Geocoding;

namespace AdvancedMap.Droid
{
	public class ResultTableAdapter : ArrayAdapter<GeocodingResult>
	{
		public readonly List<GeocodingResult> Items = new List<GeocodingResult>();

		public int Width = -1;

		public ResultTableAdapter(Context context) : base(context, -1)
		{
		}

		public override int Count { get { return Items.Count; } }

		public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			ResultTableCell cell = convertView as ResultTableCell;
			GeocodingResult item = Items[position];

			if (convertView == null)
			{
				cell = new ResultTableCell(Context);

				int height = (int)(40 * cell.Density);
				cell.LayoutParameters = new AbsListView.LayoutParams(Width, height);
				cell.SetInternalFrame(0, 0, Width, height);
			}

			cell.Update(item);

			return cell;
		}
	}

}
