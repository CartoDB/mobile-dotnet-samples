using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
	public class MapListAdapter : BaseAdapter<string>
	{
		List<Type> Items { get; set; }
		Context context;

		public override string this[int position]
		{
			get
			{
				return Items[position] + "";
			}
		}

		public override int Count
		{
			get
			{
				return Items.Count;
			}
		}

		public MapListAdapter(Context context, List<Type> items)
		{
			Items = items;
			this.context = context;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			MapRowView view = (MapRowView)convertView;
			Type item = Items[position];

			if (view == null)
			{
				view = new MapRowView(context);
			}

			view.Update(item);

			return view;
		}
	}
}

