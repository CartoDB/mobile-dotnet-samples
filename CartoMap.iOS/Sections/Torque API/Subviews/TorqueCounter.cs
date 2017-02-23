
using System;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueCounter : UILabel
	{
		public TorqueCounter()
		{
			TextColor = UIColor.White;
			Font = UIFont.FromName("Helvetica Neue", 14);
			TextAlignment = UITextAlignment.Center;

			BackgroundColor = UIColor.FromRGBA(50, 50, 50, 160);

			Layer.CornerRadius = 5;
		}

		public void Update(int frameNumber, int frameCount)
		{
			string number = "";

			if (frameCount > 100)
			{
				if (frameNumber < 10)
				{
					number = "00" + frameNumber;
				}
				else if (frameNumber < 100)
				{
					number = "0" + frameNumber;
				}
				else
				{
					number = frameNumber.ToString();
				}
			}

			Text = number + "/" + frameCount;
		}
	}
}
