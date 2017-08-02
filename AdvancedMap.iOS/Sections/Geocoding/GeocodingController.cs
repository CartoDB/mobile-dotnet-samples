
using System;

namespace AdvancedMap.iOS
{
    public class GeocodingController : BaseGeocodingController
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ContentView = new GeocodingView();
            View = ContentView;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }
    }
}
