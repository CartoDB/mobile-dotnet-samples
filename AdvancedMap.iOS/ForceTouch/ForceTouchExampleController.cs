using System;
using CoreGraphics;
using UIKit;

namespace AdvancedMap.iOS
{
	public class ForceTouchExampleController : UIViewController
	{
		UIView ContentView { get; set; }
		UILabel label; 
		ForceTouchRecognizer recognizer = new ForceTouchRecognizer();

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			label = new UILabel { Frame = new CGRect(10, 50, 300, 100) };

			ContentView = new UIView { BackgroundColor = UIColor.Yellow };
			ContentView.Add(label);
			View = ContentView;

			Title = "Force touch test";

			recognizer = new ForceTouchRecognizer();
			ContentView.AddGestureRecognizer(recognizer);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			recognizer.ForceTouch += OnForceTouch;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			recognizer.ForceTouch -= OnForceTouch;
		}

		void OnForceTouch(object sender, ForceEventArgs e)
		{
			Console.WriteLine("OnForceTouch: " + e.Type);
			label.Text = e.RoundedForce + " (" + e.Type + ")";
		}

	}
}

