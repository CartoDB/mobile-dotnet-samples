using System;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
	public class BaseActivity : Activity
	{
		protected const int RequestCode = 1;
		protected const int Marshmallow = 23;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (ActionBar != null)
			{
				ActionBar.SetDisplayHomeAsUpEnabled(true);
				ActionBar.SetBackgroundDrawable(new ColorDrawable { Color = Colors.ActionBar });
			}
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		protected void Alert(string message)
		{
			RunOnUiThread(delegate
			{
				Toast.MakeText(this, message, ToastLength.Short).Show();
			});
		}

	}
}

