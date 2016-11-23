using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Shared;

namespace AdvancedMap.Droid
{
	public class BaseMapSectionMenu : LinearLayout
	{
		public EventHandler<OptionEventArgs> SelectionChange;

		public bool IsVisible { get { return Alpha == 1.0f; } }

		List<BaseMapSectionMenuItem> views = new List<BaseMapSectionMenuItem>();
		LinearLayout contentContainer;

		List<Section> items;
		public List<Section> Items
		{
			get { return items; }
			set
			{

				items = value;

				foreach (Section section in items)
				{
					BaseMapSectionMenuItem view = new BaseMapSectionMenuItem(context);
					view.Section = section;
					views.Add(view);
					contentContainer.AddView(view);
				}
			}
		}

		Context context;

		public BaseMapSectionMenu(Context context) : base(context)
		{
			this.context = context;

			LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

			contentContainer = new LinearLayout(context);
			contentContainer.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
			contentContainer.Orientation = Orientation.Vertical;

			AddView(contentContainer);

			SetBackgroundColor(Android.Graphics.Color.Argb(130, 0, 0, 0));

			Alpha = 0.0f;
			Visibility = ViewStates.Gone;
		}

		public bool LanguageChoiceEnabled
		{
			set
			{
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

		public void Show()
		{
			Visibility = ViewStates.Visible;
			AnimateAlpha(1.0f);
		}

		public void Hide()
		{
			AnimateAlpha(0.0f, delegate { Visibility = ViewStates.Gone; });
		}

		void AnimateAlpha(float alpha, Action completionHandler = null)
		{
			Animate().Alpha(alpha).SetDuration(200).SetListener(new CompletionListener(delegate
			{
				Alpha = alpha;
				if (completionHandler != null)
				{
					completionHandler();
				}
			}));
		}

		BaseMapSectionLabel current;
		BaseMapSectionLabel currentLanguage;

		public override bool OnTouchEvent(MotionEvent e)
		{
			MotionEventActions action = e.Action;

			int x = (int)e.GetX();
			int y = (int)e.GetY();

			if (e.Action == MotionEventActions.Up)
			{
				bool isInBox = false;

				foreach (BaseMapSectionMenuItem item in views) {

					Rect outerRect = item.HitRect;

					if (outerRect.Contains(x, y))
					{
						isInBox = true;
						int headerHeight = item.HeaderHeight;

						if (!item.Enabled)
						{
							return true;
						}

						foreach (BaseMapSectionLabel option in item.Options) 
						{
							if (option.GetGlobalRect(headerHeight, outerRect).Contains(x, y))
							{
								if (item.Section.Type == SectionType.Language)
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

								if (SelectionChange != null)
								{
									SelectionChange(null, new OptionEventArgs { Section = item.Section, Option = option });
								}
							}
						}
					}
				}

				if (!isInBox)
				{
					Hide();
				}
			}

			return true;
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
	}

	public class CompletionListener : Java.Lang.Object, Animator.IAnimatorListener
	{
		Action onComplete;

		public CompletionListener(Action onComplete = null)
		{
			this.onComplete = onComplete;
		}

		public void OnAnimationCancel(Animator animation)
		{

		}

		public void OnAnimationEnd(Animator animation)
		{
			if (onComplete != null)
			{
				onComplete();
			}
		}

		public void OnAnimationRepeat(Animator animation)
		{

		}

		public void OnAnimationStart(Animator animation)
		{

		}
	}
}

