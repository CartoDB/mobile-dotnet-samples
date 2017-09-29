using System;
using Android.Content;
using Carto.Utils;
using Shared;

namespace AdvancedMap.Droid
{
    public static class RoutingExtensions
    {
        public static void SetSourcesAndElements(this Routing Routing, Context context)
        {
			Carto.Graphics.Bitmap olmarker = CreateBitmap(context, Resource.Drawable.olmarker);
			Carto.Graphics.Bitmap directionUp = CreateBitmap(context, Resource.Drawable.direction_up);
			Carto.Graphics.Bitmap directionUpLeft = CreateBitmap(context, Resource.Drawable.direction_upthenleft);
			Carto.Graphics.Bitmap directionUpRight = CreateBitmap(context, Resource.Drawable.direction_upthenright);

			Carto.Graphics.Color green = new Carto.Graphics.Color(Android.Graphics.Color.Green);
			Carto.Graphics.Color red = new Carto.Graphics.Color(Android.Graphics.Color.Red);
			Carto.Graphics.Color white = new Carto.Graphics.Color(Android.Graphics.Color.White);

			Routing.SetSourcesAndElements(olmarker, directionUp, directionUpLeft, directionUpRight, green, red, white);
		}

        public static Carto.Graphics.Bitmap CreateBitmap(Context context, int resource)
		{
			return BitmapUtils.CreateBitmapFromAndroidBitmap(Android.Graphics.BitmapFactory.DecodeResource(context.Resources, resource));
		}

	}
}
