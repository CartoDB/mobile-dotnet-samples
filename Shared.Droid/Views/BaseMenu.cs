using System;
using Android.Animation;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
	public class BaseMenu : LinearLayout
	{
		public bool IsVisible { get { return Alpha == 1.0f; } }

		public BaseMenu(Context context) : base(context)
		{
			LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

			Alpha = 0.0f;
			Visibility = ViewStates.Gone;
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

	// Animator requires API 11 (Android 3.0)
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
