
using System;
using UIKit;

namespace Shared.iOS
{
    public class Device
    {
        public static bool IsLandscape
        {
            get
            {
                var orientation = UIDevice.CurrentDevice.Orientation;
                return orientation == UIDeviceOrientation.LandscapeLeft || orientation == UIDeviceOrientation.LandscapeRight;
            }
        }

        public static bool IsTablet
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; }
        }

        public static nfloat NavBarHeight
        {
            get
            {

#if ADVANCED_IOS
                return (UIApplication.SharedApplication.Delegate as AdvancedMap.iOS.AppDelegate).Controller.NavigationBar.Frame.Height;
#elif CARTO_IOS
                return (UIApplication.SharedApplication.Delegate as CartoMap.iOS.AppDelegate).Controller.NavigationBar.Frame.Height;
#elif HELLO_IOS
                // TODO return (UIApplication.SharedApplication.Delegate as HelloMap.iOS.AppDelegate).Controller.NavigationBar.Frame.Height;
#endif

				return -1;

            }
        }

        public static nfloat StatusBarHeight
        {
            get { return UIApplication.SharedApplication.StatusBarFrame.Height; }
        }
    }
}
