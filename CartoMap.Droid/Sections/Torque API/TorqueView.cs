
using System;
using Android.Content;
using Android.Widget;
using Carto.Ui;

namespace CartoMap.Droid
{
	public class TorqueView : RelativeLayout
	{
		public MapView MapView { get; private set; }

		public PlayButton Button { get; private set; }

		public TorqueCounter Counter { get; private set; }

		public TorqueView(Context context) : base(context)
		{
			MapView = new MapView(context);
			AddView(MapView);

			MapView.LayoutParameters = new RelativeLayout.LayoutParams(
				RelativeLayout.LayoutParams.MatchParent, 
				RelativeLayout.LayoutParams.MatchParent
			);

			Button = new PlayButton(context);
			AddView(Button);

			Counter = new TorqueCounter(context);
			AddView(Counter);
		}
	}
}
