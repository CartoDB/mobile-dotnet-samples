
using System;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PackageManagerButton : UIButton
	{
		public string PackageId { get; set; }

		public string PackageName { get; set; }

		public int PriorityIndex { get; set; }

		public PackageManagerButtonType Type { get; set; }

		public string Text {
			get {
				return Title(UIControlState.Normal);
			} set {
				SetTitle(value, UIControlState.Normal);
			}
		}

		public PackageManagerButton ()
		{
			BackgroundColor = OptionsSelect.AppleBlue;
			SetTitleColor(UIColor.White, UIControlState.Normal);
			Layer.CornerRadius = 5;
			HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
			TitleEdgeInsets = new UIEdgeInsets(0, 0, 0, 10);
		}
	}
}

