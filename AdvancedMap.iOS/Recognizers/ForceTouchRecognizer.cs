
using System;
using Carto.Ui;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdvancedMap.iOS
{
	public class ForceTouchRecognizer : UITapGestureRecognizer
	{
		bool isEventHandled;

		public EventHandler<ForceEventArgs> ForceTouch;

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			UITouch touch = (UITouch)touches.AnyObject;

			// Arbitrary number; let's say at least 1 to differentiate it from a normal touch
			if (touch.Force > 1) 
			{
				if (ForceTouch != null && !isEventHandled) 
				{
					isEventHandled = true;
					ForceTouch(View, new ForceEventArgs(touch.Force));
				}
			}

			base.TouchesMoved(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			isEventHandled = false;
			base.TouchesEnded(touches, evt);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent evt)
		{
			isEventHandled = false;
			base.TouchesCancelled(touches, evt);
		}
	}

	public enum ForceType
	{
		None,
		Weak,
		Medium,
		Strong
	}

	public class ForceEventArgs : EventArgs
	{
		public ForceType Type;

		public ForceEventArgs(nfloat force)
		{
			// TODO These numbers may need to be tweaked
			if (force > 5)
			{
				Type = ForceType.Strong;
			}
			else if (force > 3)
			{
				Type = ForceType.Medium;
			}
			else 
			{
				Type = ForceType.Weak;
			}
		}
	}
}

