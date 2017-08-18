using System;
using Carto.Graphics;
using Carto.Utils;
using Shared;
using UIKit;

namespace AdvancedMap.iOS
{
    public static class RoutingExtensions
    {
        public static void SetSourcesAndElements(this Routing Routing)
        {
			Bitmap olmarker = CreateBitmap("icons/olmarker.png");
			Bitmap directionUp = CreateBitmap("icons/direction_up.png");
			Bitmap directionUpLeft = CreateBitmap("icons/direction_upthenleft.png");
			Bitmap directionUpRight = CreateBitmap("icons/direction_upthenright.png");

			Color green = new Color(0, 255, 0, 255);
			Color red = new Color(255, 0, 0, 255);
			Color white = new Color(255, 255, 255, 255);

			Routing.SetSourcesAndElements(olmarker, directionUp, directionUpLeft, directionUpRight, green, red, white);
        }

		public static Bitmap CreateBitmap(string resource)
		{
			return BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile(resource));
		}

	}
}
