
using System;
using Carto.Core;
using Carto.Ui;

namespace CartoMobileSample
{
	/**
	 * This MapListener waits for two clicks on map - first to set routing start point, and then
	 * second to mark end point and start routing service.
	 */
	public class RouteMapEventListener : MapEventListener
	{
		MapPos startPos;
		MapPos stopPos;

		public EventHandler<RouteMapEventArgs> StartPositionClicked;
		public EventHandler<RouteMapEventArgs> StopPositionClicked;

		// Map View manipulation handlers
		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			if (mapClickInfo.ClickType == ClickType.ClickTypeLong)
			{
				MapPos clickPos = mapClickInfo.ClickPos;

				if (startPos == null)
				{
					// Set start, or start again
					startPos = clickPos;

					if (StartPositionClicked != null) {
						StartPositionClicked(new object(), new RouteMapEventArgs { ClickPosition = clickPos });
					}
				}
				else if (stopPos == null)
				{
					// Set stop and calculate
					stopPos = clickPos;

					if (StartPositionClicked != null)
					{
						StopPositionClicked(new object(), new RouteMapEventArgs { 
							ClickPosition = clickPos,
							StartPosition = startPos,
							StopPosition = stopPos
						});
					}

					// Restart to force new route next time
					startPos = null;
					stopPos = null;
				}
			}
		}

		public override void OnMapMoved()
		{

		}
	}

	public class RouteMapEventArgs : EventArgs
	{
		public MapPos StartPosition { get; set; }

		public MapPos StopPosition { get; set; }

		public MapPos ClickPosition { get; set; }
	}
}

