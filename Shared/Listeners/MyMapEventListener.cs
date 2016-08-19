
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
	/**
	* A custom map event listener that displays information about map events and creates pop-ups.
	*/
	public class MyMapEventListener : MapEventListener
	{
		MapView mapView;
		LocalVectorDataSource vectorDataSource;

		BalloonPopup oldClickLabel;

		public MyMapEventListener(MapView mapView, LocalVectorDataSource vectorDataSource)
		{
			this.mapView = mapView;
			this.vectorDataSource = vectorDataSource;
		}

		public override void OnMapMoved()
		{
			//MapPos topLeft = mapView.ScreenToMap(new ScreenPos(0, 0));
			//MapPos bottomRight = mapView.ScreenToMap(new ScreenPos(mapView.Width, mapView.Height));

			//MapPos mapPos = mapView.Options.BaseProjection.FromWgs84(new MapPos(0, 0));
			//ScreenPos screenPos = mapView.MapToScreen(mapPos);
		}

		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			// Remove old click label
			if (oldClickLabel != null)
			{
				vectorDataSource.Remove(oldClickLabel);
				oldClickLabel = null;
			}

			BalloonPopupStyleBuilder styleBuilder = new BalloonPopupStyleBuilder();
			// Make sure this label is shown on top all other labels
			styleBuilder.PlacementPriority = 10;

			// Check the type of the click
			string clickMsg = null;

			if (mapClickInfo.ClickType == ClickType.ClickTypeSingle)
			{
				clickMsg = "Single map click!";
			}
			else if (mapClickInfo.ClickType == ClickType.ClickTypeLong)
			{
				clickMsg = "Long map click!";
			}
			else if (mapClickInfo.ClickType == ClickType.ClickTypeDouble)
			{
				clickMsg = "Double map click!";
			}
			else if (mapClickInfo.ClickType == ClickType.ClickTypeDual)
			{
				clickMsg = "Dual map click!";
			}

			MapPos clickPos = mapClickInfo.ClickPos;

			// Show click coordinates also
			MapPos wgs84Clickpos = mapView.Options.BaseProjection.ToWgs84(clickPos);
			//string message = string.Format("%.4f, %.4f", wgs84Clickpos.Y, wgs84Clickpos.X);
			string message = Math.Round(wgs84Clickpos.Y, 4) + ", " + Math.Round(wgs84Clickpos.X, 4);

			BalloonPopupStyle style = styleBuilder.BuildStyle();
			BalloonPopup popup = new BalloonPopup(mapClickInfo.ClickPos, style, clickMsg, message);

			vectorDataSource.Add(popup);

			oldClickLabel = popup;
		}

	}
}

