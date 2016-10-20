
using System;
using System.Threading.Tasks;
using UIKit;

namespace Shared.iOS
{
	public class BaseController : GLKit.GLKViewController
	{
		public virtual string Name { get; set; }

		public virtual new string Description { get; set; }

		protected async void Alert(string message)
		{
			await ShowToast(message);
		}

		async Task ShowToast(string message, UIAlertView toast = null)
		{
			if (toast == null)
			{
				toast = new UIAlertView(null, message, null, null, null);
				toast.Show();
				await Task.Delay(1 * 1000);
				await ShowToast(message, toast);
				return;
			}

			UIView.BeginAnimations("");
			toast.Alpha = 0;
			UIView.CommitAnimations();
			toast.DismissWithClickedButtonIndex(0, true);
		}
	}
}

