
using System;
using Android.Animation;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.Layers;
using Carto.Ui;

namespace AdvancedMap.Droid
{
	public class BaseMapsView : RelativeLayout
	{
		public MapView MapView { get; set; }

		public MenuButton Button { get; set; }

		public OptionsMenu Menu { get; set; }

		public BaseMapsView(Context context) : base(context)
		{
			MapView = new MapView(context);

			var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(baseLayer);

			MapView.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

			AddView(MapView);

			Button = new MenuButton(context);
			AddView(Button);

			Menu = new OptionsMenu(context);
			AddView(Menu);
		}
	}
}

