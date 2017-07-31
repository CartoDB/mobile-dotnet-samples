
using System;

namespace AdvancedMap.iOS
{
    public class ReverseGeocodingController : BaseGeocodingController
    {
        public ReverseGeocodingController()
        {
            ContentView = new ReverseGeocodingView();
        }
    }
}
