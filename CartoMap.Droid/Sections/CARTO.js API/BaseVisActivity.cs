using System;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	public class BaseVisActivity : MapBaseActivity
	{
		protected void UpdateVis(string url)
		{
			MapView.UpdateVisWithGridEvent(url);
		}

	}
}

