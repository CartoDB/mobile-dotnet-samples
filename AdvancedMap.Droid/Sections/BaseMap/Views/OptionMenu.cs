using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Content;
using Android.Views;
using Android.Widget;
using Shared;

namespace AdvancedMap.Droid
{
	public class OptionMenu : LinearLayout
	{
		public bool IsVisible { get { return Alpha == 1.0f; } }

		List<OptionMenuItem> views = new List<OptionMenuItem>();
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
					OptionMenuItem view = new OptionMenuItem(context);
					view.Section = section;
					views.Add(view);
					contentContainer.AddView(view);
				}
			}
		}

		Context context;

		public OptionMenu(Context context) : base(context)
		{
			this.context = context;

			LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

			contentContainer = new LinearLayout(context);
			contentContainer.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
			contentContainer.Orientation = Orientation.Vertical;

			AddView(contentContainer);

			SetBackgroundColor(Android.Graphics.Color.Argb(130, 0, 0, 0));

			Alpha = 0.0f;
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

