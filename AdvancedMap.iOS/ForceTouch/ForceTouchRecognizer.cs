
using System;
using System.Collections.Generic;
using Carto.Ui;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdvancedMap.iOS
{
	public class ForceTouchRecognizer : UITapGestureRecognizer
	{
		public EventHandler<ForceEventArgs> ForceTouch;

		List<nfloat> forces = new List<nfloat>();

		public nfloat AverageForce { 
			get {
				nfloat total = 0;

				foreach (nfloat force in forces)
				{
					total += force;
				}

				return total / forces.Count;
			}
		}

		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			forces.Clear();

			// Base call is important, else this recognizer consumes the event and the SDK won't receive it
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			// Touches moved captures the actual force of it, 
			// shouldn't be caught on TouchesEnded as that catches force during the end of the touch
			UITouch touch = (UITouch)touches.AnyObject;
			forces.Add(touch.Force);

			// Base call is important, else this recognizer consumes the event and the SDK won't receive it
			base.TouchesMoved(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			UITouch touch = (UITouch)touches.AnyObject;

			if (ForceTouch != null)
			{
				// Be sure to grab the average force of the entire touch event to determine full force
				ForceTouch(View, new ForceEventArgs(AverageForce));
			}

			// Base call is important, else this recognizer consumes the event and the SDK won't receive it
			base.TouchesEnded(touches, evt);
		}

	}

	public enum ForceType
	{
		Weak,
		Medium,
		Strong
	}

	public class ForceEventArgs : EventArgs
	{
		public bool IsForce { get { return Type != ForceType.Weak; } }

		public nfloat Force { get; set; }

		public double RoundedForce { get { return Math.Round(Force, 2); } }

		public ForceType Type { get; set; }

		public ForceEventArgs(nfloat force)
		{
			Force = force;

			// TODO Tweak these numbers
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

