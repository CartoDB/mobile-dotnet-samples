
using System;
using Android.Widget;
using Android.Content;
using Carto.Ui;

namespace HelloMap.Forms.Droid
{
	public class MapContainer : RelativeLayout
	{
		public MapView Map { get; private set; }

		public MapContainer(Context context) : base(context)
		{
			SetBackgroundColor(Android.Graphics.Color.Red);

			Map = new MapView(context);

			Map.LayoutParameters = new RelativeLayout.LayoutParams(
				RelativeLayout.LayoutParams.MatchParent, 
				RelativeLayout.LayoutParams.MatchParent
			);

			AddView(Map);
		}
	}
}
