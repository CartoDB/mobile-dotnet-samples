
using System;
using Android.Animation;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.Layers;
using Carto.Ui;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class BaseMapsView : MapWithMenuButton
	{
		public BaseMapSectionMenu Menu { get; set; }

		public BaseMapsView(Context context) : base(context, Resource.Drawable.icon_menu_round)
		{
            var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
			MapView.Layers.Add(baseLayer);

			Menu = new BaseMapSectionMenu(context);
			AddView(Menu);
		}
	}
}

