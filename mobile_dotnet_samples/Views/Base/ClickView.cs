
using System;
using UIKit;

namespace Shared.iOS.Views.Base
{
    public class ClickView : UIView
    {
        public EventHandler<EventArgs> Click;

        public bool IsEnabled { get; private set; } = true;

        public void Enable()
        {
            IsEnabled = true;
            Alpha = 1.0f;
        }

        public void Disable()
        {
            IsEnabled = false;
            Alpha = 0.5f;
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            Alpha = 0.5f;
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            if (!IsEnabled)
            {
                return;
            }

            Click?.Invoke(this, EventArgs.Empty);
            Alpha = 1.0f;
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            if (!IsEnabled)
            {
                return;
            }

            Alpha = 1.0f;
        }
    }
}
