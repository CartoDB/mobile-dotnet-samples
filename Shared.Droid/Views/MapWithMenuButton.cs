using System;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.Ui;

namespace Shared.Droid
{
	public class MapWithMenuButton : RelativeLayout
	{
		public MapView MapView { get; set; }

		public MenuButton Button { get; set; }

		public MapWithMenuButton(Context context, int resource) : base(context)
		{
			MapView = new MapView(context);

			MapView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

			AddView(MapView);

			Button = new MenuButton(resource, context);
			AddView(Button);
		}

	}
}
