
using System;
using Carto.Graphics;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
	public class HelloMapEventListener : MapEventListener
	{
		Color[] colors = {
			new Color(255, 255, 255, 255), // White
			new Color(0, 0, 255, 255), // Blue
		    new Color(255, 0, 0, 255), // Red
		    new Color(0, 255, 0, 255), // Green
		    new Color(0, 0, 0, 255) // Black
		};

		Marker marker;

		Random random;

		public HelloMapEventListener(Marker marker)
		{
			this.marker = marker;
			random = new Random();
		}

		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			base.OnMapClicked(mapClickInfo);

			MarkerStyleBuilder builder = new MarkerStyleBuilder();

			// Set random size (within reasonable limits)
			int size = GetRandomInt(15, 50);
			builder.Size = size;

			// Set random color from our list
			builder.Color = colors[GetRandomInt(0, colors.Length)];

			// Set a new style to the marker
			marker.Style = builder.BuildStyle();
		}

		int GetRandomInt(int min, int max)
		{
			return random.Next(max - min) + min;
		}
	}
}

