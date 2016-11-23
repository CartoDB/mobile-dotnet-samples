
using System;
using System.Collections.Generic;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class BaseMapSectionMenu : BaseMenu
	{
		public EventHandler<OptionEventArgs> OptionTapped;

		static nfloat SmallBoxPadding = 5;

		static nfloat LargeBoxPadding = 20;

		List<BaseMapSectionMenuItem> views;

		List<Section> items;
		public List<Section> Items
		{
			get { return items; }
			set {

				items = value;

				foreach (Section section in items)
				{
					BaseMapSectionMenuItem view = new BaseMapSectionMenuItem();
					view.Section = section;
					view.AddGestureRecognizer(new UITapGestureRecognizer(OnSubviewTap));
					views.Add(view);
					AddSubview(view);

					view.OptionTapped += OnOptionTap;
				}
			}
		}

		public BaseMapSectionMenu()
		{
			BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);

			views = new List<BaseMapSectionMenuItem>();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = SmallBoxPadding;
			nfloat y = LargeBoxPadding;
			nfloat w = Frame.Width - 2 * SmallBoxPadding;
			nfloat h = 0;

			int counter = 0;

			foreach (BaseMapSectionMenuItem view in views)
			{
				// Trivial heights
				h = view.IsMultiLine ? 120 : 80;
				view.Frame = new CGRect(x, y, w, h);

				if (counter == 2)
				{
					// Extra padding so language menu would be more separate
					y += h + LargeBoxPadding;
				}
				else {
					y += h + SmallBoxPadding;
				}

				counter++;
			}
		}

		void OnSubviewTap()
		{
			// Do nothing. Just catch taps
			Console.WriteLine("Subview (box) tapped");
		}

		BaseMapSectionLabel current;
		BaseMapSectionLabel currentLanguage;

		void OnOptionTap(object sender, OptionEventArgs e)
		{
			BaseMapSectionLabel option = e.Option;

			if (e.Section.Type == SectionType.Language)
			{
				if (currentLanguage != null)
				{
					currentLanguage.Normalize();
				}

				option.Highlight();

				currentLanguage = option;
			}
			else 
			{
				if (current != null)
				{
					current.Normalize();
				}

				option.Highlight();

				current = option;
			}

			if (OptionTapped != null)
			{
				OptionTapped(null, e);
			}
		}

		public void SetInitialItem(Section section)
		{
			foreach (BaseMapSectionMenuItem view in views)
			{
				if (view.Section.Equals(section))
				{
					if (section.Type == SectionType.Language)
					{
						currentLanguage = view.SetFirstItemActive();
					}
					else
					{
						current = view.SetFirstItemActive();
					}
				}
			}
		}

		public bool LanguageChoiceEnabled 
		{ 
			set {
				BaseMapSectionMenuItem language = views.Find(item => item.section.Type == SectionType.Language);

				if (language == null)
				{
					return;
				}

				if (value)
				{
					language.Enabled = true;
				}
				else
				{
					language.Enabled = false;
				}
			}
		}
	}

	public class OptionEventArgs : EventArgs
	{
		public Section Section { get; set; }

		public BaseMapSectionLabel Option { get; set; }
	}
}

