
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class BaseMapSectionLabel : UILabel
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsActive { get { return BackgroundColor != UIColor.White; } }

		public BaseMapSectionLabel(NameValuePair option)
		{
			Name = option.Name;
			Value = option.Value;

			Text = option.Name.ToUpper();

			TextAlignment = UITextAlignment.Center;

			Font = UIFont.FromName("HelveticaNeue-Bold", 11);

			Layer.BorderWidth = 0.5f;
		}

		public void Highlight()
		{
			BackgroundColor = Colors.AppleBlue;
			Layer.BorderColor = Colors.AppleBlue.CGColor;
			TextColor = UIColor.White;
		}

		public void Normalize()
		{
			BackgroundColor = UIColor.White;
			Layer.BorderColor = UIColor.FromRGB(50, 50, 50).CGColor;
			TextColor = UIColor.FromRGB(50, 50, 50);
		}

	}
}

