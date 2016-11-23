
using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class CityChoiceMenu : BaseMenu
	{
		public EventHandler<EventArgs> CityTapped;

		List<CityLabel> views = new List<CityLabel>();

		List<BoundingBox> items;
		public List<BoundingBox> Items 
		{ 
			get { return items; }
			set {
				items = value;

				foreach (BoundingBox item in items)
				{
					CityLabel view = new CityLabel();
					view.BoundingBox = item;
					view.AddGestureRecognizer(new UITapGestureRecognizer(OnSubviewTap));
					views.Add(view);
					AddSubview(view);
				}
			}
		}

		public CityChoiceMenu()
		{
		}

		public override void LayoutSubviews()
		{
			nfloat w = Frame.Width / 2;
			nfloat h = w / 4;
			nfloat x = Frame.Width - w;
			nfloat y = Device.StatusBarHeight + Device.NavigationBarHeight;

			foreach (CityLabel view in views)
			{
				view.Frame = new CGRect(x, y, w, h);
				// Move to next position, but remove double borders
				y += h - view.Layer.BorderWidth;
			}

			base.LayoutSubviews();
		}

		public void OnSubviewTap(UITapGestureRecognizer recognizer)
		{
			CityLabel view = (CityLabel)recognizer.View;

			if (CityTapped != null)
			{
				CityTapped(view.BoundingBox, EventArgs.Empty);
			}
		}
	}

	public class CityLabel : UILabel
	{
		BoundingBox box;
		public BoundingBox BoundingBox 
		{ 
			get { return box; } 
			set {
				box = value;

				Text = box.Name.ToUpper();
			}
		}

		public CityLabel()
		{
			BackgroundColor = UIColor.FromRGB(240, 240, 240);
			TextAlignment = UITextAlignment.Center;

			Layer.BorderWidth = 0.5f;
			Layer.BorderColor = UIColor.Gray.CGColor;

			Font = UIFont.FromName("HelveticaNeue-Light", 13);

			UserInteractionEnabled = true;
		}
	}
}
