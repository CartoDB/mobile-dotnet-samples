﻿
using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OptionsMenuItem : UIView
	{
		public EventHandler<OptionEventArgs> OptionTapped;

		protected UIView headerContainer;
		protected UIView contentContainer;

		protected UILabel osmLabel, separator, tileTypeLabel;

		protected List<OptionLabel> optionLabels;

		public bool IsMultiLine { get { return section.Styles.Count > 3; } }

		public Section section;
		public Section Section
		{
			get { return section; }
			set
			{
				section = value;

				osmLabel.Text = section.OSM.Name.ToUpper();
				tileTypeLabel.Text = "TYPE: " + section.Type.ToString().ToUpper();

				foreach (NameValuePair option in section.Styles)
				{
					OptionLabel optionLabel = new OptionLabel(option);

					optionLabels.Add(optionLabel);
					contentContainer.AddSubview(optionLabel);
				}

				LayoutSubviews();
			}
		}

		public OptionsMenuItem()
		{
			optionLabels = new List<OptionLabel>();

			Layer.CornerRadius = 5;
			Layer.ShadowOpacity = 0.5f;
			Layer.ShadowOffset = new CGSize(-15, 20);
			Layer.ShadowRadius = 5;
			Layer.MasksToBounds = true;

			headerContainer = new UIView();
			headerContainer.BackgroundColor = Colors.AppleBlue;

			headerContainer.Layer.ShadowOpacity = 0.5f;
			headerContainer.Layer.ShadowOffset = new CGSize(0, 2);
			headerContainer.Layer.ShadowRadius = 2;

			contentContainer = new UIView();
			contentContainer.BackgroundColor = UIColor.FromRGB(240, 240, 240);
			contentContainer.ClipsToBounds = true;

			AddSubviews(contentContainer, headerContainer);

			osmLabel = new UILabel();
			osmLabel.TextColor = UIColor.White;
			osmLabel.Font = UIFont.FromName("HelveticaNeue", 14);
			osmLabel.TextAlignment = UITextAlignment.Justified;

			separator = new UILabel { BackgroundColor = UIColor.White };

			tileTypeLabel = new UILabel();
			tileTypeLabel.TextColor = UIColor.White;
			tileTypeLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12);
			tileTypeLabel.TextAlignment = UITextAlignment.Center;

			headerContainer.AddSubviews(osmLabel, separator, tileTypeLabel);

			contentContainer.AddGestureRecognizer(new UITapGestureRecognizer(OnContainerTap));
		}

		protected nfloat padding = 10;
		protected nfloat smallPadding = 6;

		protected nfloat separatorWidth = 1;
		protected nfloat separatorPadding = 7;

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat height = 40;
			nfloat itemHeight = 28;

			headerContainer.Frame = new CGRect(0, 0, Frame.Width, height);
			contentContainer.Frame = new CGRect(0, height, Frame.Width, Frame.Height - height);

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = headerContainer.Frame.Width / 2 - (2 * padding + separatorWidth);
			nfloat h = headerContainer.Frame.Height;

			osmLabel.Frame = new CGRect(x, y, w, h);

			x += w + padding;

			separator.Frame = new CGRect(x, separatorPadding, separatorWidth, h - 2 * separatorPadding);

			x += separatorWidth + padding;

			tileTypeLabel.Frame = new CGRect(x, y, w, h);

			x = padding;
			y = smallPadding;

			if (IsMultiLine)
			{
				// One third, taking padding into account
				w = (contentContainer.Frame.Width - (padding + 3 * padding)) / 3;
				h = itemHeight;

				int counter = 0;

				foreach (OptionLabel label in optionLabels)
				{
					if (counter > 0 && counter % 3 == 0)
					{
						y += h + padding;
						x = padding;
					}

					label.Frame = new CGRect(x, y, w, h);
					x += w + padding;

					counter++;
				}
			}
			else 
			{
				w = (contentContainer.Frame.Width - (padding + optionLabels.Count * padding)) / optionLabels.Count;
				h = itemHeight;

				foreach (OptionLabel label in optionLabels)
				{
					label.Frame = new CGRect(x, y, w, h);
					x += w + padding;
				}
			}
		}

		void OnContainerTap(UITapGestureRecognizer recognizer)
		{
			CGPoint point = recognizer.LocationInView(contentContainer);

			foreach (OptionLabel label in optionLabels)
			{
				if (label.Frame.Contains(point) && OptionTapped != null)
				{
					OptionTapped(null, new OptionEventArgs { Section = this.Section, Option = label });
				}
			}
		}

		public OptionLabel SetFirstItemActive()
		{
			if (optionLabels.Count > 0)
			{
				OptionLabel label = optionLabels[0];
				label.Highlight();
				return label;
			}

			return null;
		}

	}
}
