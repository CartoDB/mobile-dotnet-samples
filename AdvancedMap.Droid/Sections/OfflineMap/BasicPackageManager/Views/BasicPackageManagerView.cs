
using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class BasicPackageManagerView : MapWithMenuButton
	{
		public TextView StatusLabel { get; private set; }

		public CityChoiceMenu Menu { get; set; }

		public BasicPackageManagerView(Context context) : base(context, Resource.Drawable.icon_menu_round)
		{
			// Initialize & style Status label
			StatusLabel = new TextView(context);
			StatusLabel.SetTextColor(Color.Black);

			GradientDrawable background = new GradientDrawable();
			background.SetCornerRadius(5);
			background.SetColor(Color.Argb(160, 255, 255, 255));
			StatusLabel.Background = background;

			StatusLabel.Gravity = Android.Views.GravityFlags.Center;
			StatusLabel.Typeface = Typeface.Create("HelveticaNeue", TypefaceStyle.Normal);

			DisplayMetrics screen = Resources.DisplayMetrics;

			int width = screen.WidthPixels / 2;
			int height = width / 4;

			int x = screen.WidthPixels / 2 - width / 2;
			int y = screen.HeightPixels / 100;

			var parameters = new RelativeLayout.LayoutParams(width, height);
			parameters.TopMargin = y;
			parameters.LeftMargin = x;

			AddView(StatusLabel, parameters);

			Menu = new CityChoiceMenu(context);
			AddView(Menu);
		}

		public void ZoomTo(MapPos position)
		{
			MapView.FocusPos = MapView.Options.BaseProjection.FromWgs84(position);
			MapView.SetZoom(12, 2);
		}

		public void ZoomOut()
		{
			MapView.SetZoom(0, 2);
		}

		public void SetBaseLayer(Carto.PackageManager.CartoPackageManager manager)
		{
			var layer = new CartoOfflineVectorTileLayer(manager, CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(layer);
		}
	}
}
