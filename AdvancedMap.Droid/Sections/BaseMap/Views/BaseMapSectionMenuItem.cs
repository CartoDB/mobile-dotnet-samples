using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class BaseMapSectionMenuItem : LinearLayout
	{
		LinearLayout headerContainer;
		RelativeLayout contentContainer;

		public bool IsMultiLine { get { return section.Styles.Count > 3; } }

		TextView osmLabel, separatorLabel, tileTypeLabel;

		List<BaseMapSectionLabel> optionLabels = new List<BaseMapSectionLabel>();

		public List<BaseMapSectionLabel> Options { get { return optionLabels; } }

		public Rect HitRect 
		{ 
			get {
				var rect = new Rect();
				GetHitRect(rect);

				return rect;
			} 
		}

		int headerHeight;
		public int HeaderHeight {
			get {
				if (headerHeight == 0) {
					headerContainer.Measure(0, 0);
					headerHeight = headerContainer.MeasuredHeight;
				}

				return headerHeight;
			}
		}

		public Section section;
		public Section Section
		{
			get { return section; }
			set
			{
				section = value;

				osmLabel.Text = section.OSM.Name.ToUpper();
				separatorLabel.Text = "|";
				tileTypeLabel.Text = section.Type.ToString().ToUpper();

				foreach (NameValuePair option in section.Styles)
				{
					BaseMapSectionLabel optionLabel = new BaseMapSectionLabel(context, option);

					optionLabels.Add(optionLabel);
					contentContainer.AddView(optionLabel);
				}

				contentContainer.Measure(0, 0);

				int rowHeight = Metrics.HeightPixels / 16;

				if (optionLabels.Count <= 3)
				{
					contentContainer.LayoutParameters.Height = rowHeight;
				}
				else if (optionLabels.Count <= 6)
				{
					contentContainer.LayoutParameters.Height = 2 * rowHeight;
				}
				else if (optionLabels.Count <= 9)
				{
					contentContainer.LayoutParameters.Height = 3 * rowHeight;
				}
				else
				{
					// Not supported
				}

				// TODO - Refactor this clusterfuck

				int counter = 1;
				int y = 0;

				foreach (BaseMapSectionLabel label in optionLabels)
				{
					label.SetRelativeLayout(contentContainer.MeasuredWidth, rowHeight, optionLabels.Count, counter, y);

					if (counter % 3 == 0)
					{
						y += rowHeight;
					}
					counter++;
				}

				if (Section.Type == SectionType.Language)
				{
					(LayoutParameters as LinearLayout.LayoutParams).TopMargin = Padding * 2;
				}
				else if (Section.OSM.Value == "nutiteq.osm")
				{
					// Initial item, set additional padding
					(LayoutParameters as LinearLayout.LayoutParams).TopMargin = Padding;
				}
				else
				{
					(LayoutParameters as LinearLayout.LayoutParams).TopMargin = Padding / 3;
				}
			}
		}

		DisplayMetrics Metrics { get { return context.Resources.DisplayMetrics; } }

		int Padding { get { return (int)(Metrics.WidthPixels * 0.05); } }

		Context context;

		public BaseMapSectionMenuItem(Context context) : base(context)
		{
			Orientation = Orientation.Vertical;

			this.context = context;

			int width = (int)(Metrics.WidthPixels * 0.9);

			headerContainer = new LinearLayout(context);
			headerContainer.SetBackgroundColor(Colors.ActionBar);
			headerContainer.Orientation = Orientation.Horizontal;

			AddView(headerContainer);

			contentContainer = new RelativeLayout(context);
			contentContainer.LayoutParameters = new RelativeLayout.LayoutParams(width, 100);
			//contentContainer.Orientation = Orientation.Horizontal;
			AddView(contentContainer);

			osmLabel = GetHeaderItem(TypefaceStyle.Bold);
			separatorLabel = GetHeaderItem(TypefaceStyle.Bold);
			separatorLabel.Gravity = Android.Views.GravityFlags.Center;

			tileTypeLabel = GetHeaderItem(TypefaceStyle.Normal);

			headerContainer.AddView(osmLabel);
			headerContainer.AddView(separatorLabel);
			headerContainer.AddView(tileTypeLabel);

			var parameters = new LinearLayout.LayoutParams(width, LayoutParams.WrapContent, 0.8f);
			parameters.LeftMargin = Padding;
			parameters.RightMargin = Padding;
			parameters.TopMargin = Padding;

			LayoutParameters = parameters;
		}

		TextView GetHeaderItem(TypefaceStyle style)
		{
			TextView view = new TextView(context);

			var parameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent, 0.33f);
			parameters.LeftMargin = Padding;
			view.LayoutParameters = parameters;

			view.Typeface = Typeface.Create("Helvetica Neue", style);
			view.Gravity = Android.Views.GravityFlags.CenterVertical;
			view.SetTextColor(Color.White);
			view.SetPadding(0, 20, 0, 20);

			return view;
		}

		public BaseMapSectionLabel SetFirstItemActive()
		{
			foreach (BaseMapSectionLabel label in optionLabels)
			{
				label.Normalize();
			}

			if (optionLabels.Count > 0)
			{
				BaseMapSectionLabel label = optionLabels[0];
				label.Highlight();
				return label;
			}

			return null;
		}


		// All fields enabled by default
		bool enabled = true;

		public new bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;

				if (enabled)
				{
					Alpha = 1f;
				}
				else
				{
					Alpha = 0.5f;
				}
			}
		}

	}
}

