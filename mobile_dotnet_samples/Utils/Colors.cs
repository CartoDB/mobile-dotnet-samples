
using System;
using Carto.Graphics;
using UIKit;

namespace Shared.iOS
{
	public class Colors
	{
		public static UIColor AppleBlue = UIColor.FromRGB(0, 122, 255);

		public static UIColor CartoRed = UIColor.FromRGB(242, 68, 64);

		public static UIColor CartoRedLight = UIColor.FromRGB(215, 82, 75);

		public static UIColor CartoNavy = UIColor.FromRGB(22, 41, 69);

		public static UIColor CartoNavyTransparent = UIColor.FromRGBA(22, 41, 69, 150);

        public static UIColor TransparentGray = UIColor.FromRGBA(50, 50, 50, 150);

        public static UIColor DarkTransparentGray = UIColor.FromRGBA(50, 50, 50, 200);

        public static UIColor LightTransparentGray = UIColor.FromRGBA(50, 50, 50, 100);

        public static UIColor NearWhite = UIColor.FromRGB(245, 245, 245);
	}

    public static class ColorExtensions
    {
        public static Color ToCartoColor(this UIColor color)
        {
            nfloat red, green, blue, alpha;
            color.GetRGBA(out red, out green, out blue, out alpha);
            return new Color((byte)(red * 255.0f), (byte)(green * 255.0f), (byte)(blue * 255.0f), (byte)(alpha * 255.0f));
        }
    }
}

