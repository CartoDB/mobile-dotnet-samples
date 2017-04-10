
using System;
using Shared;
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

			BackgroundColor = TorqueHistogram.Color;

			Layer.CornerRadius = 3;
			ClipsToBounds = true;
		}

		public void Update(int frameNumber, int frameCount)
		{
			if (!TorqueUtils.Initialized)
			{
				TorqueUtils.Initialize();
			}

			Text = TorqueUtils.GetText(frameNumber);
		}

		public void Update(int frameNumber)
		{
			if (!Text.Contains("/"))
			{
				return;
			}

			int frameCount = 0;

			bool success = int.TryParse(Text.Split('/')[1], out frameCount);

			if (!success)
			{
				return;
			}

			Update(frameNumber, frameCount);
		}
	}
}
