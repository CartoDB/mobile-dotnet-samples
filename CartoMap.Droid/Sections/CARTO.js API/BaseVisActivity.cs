using System;
using Carto.Layers;
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

		protected override void OnDestroy()
		{
			TileLayer layer = MapView.FindTileLayer();

			if (layer != null)
			{
				layer.UTFGridEventListener = null;
			}

			base.OnDestroy();
		}

	}
}

