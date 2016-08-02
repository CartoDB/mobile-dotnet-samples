using System;
using Carto.Core;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;

namespace CartoMobileSample
{
	public class MyClusterElementBuilder : ClusterElementBuilder
	{
		BalloonPopupStyleBuilder balloonPopupStyleBuilder;

		public MyClusterElementBuilder()
		{
			balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			balloonPopupStyleBuilder.CornerRadius = 3;
			balloonPopupStyleBuilder.TitleMargins = new BalloonPopupMargins(6, 6, 6, 6);
			balloonPopupStyleBuilder.LeftColor = new Color(240, 230, 140, 255);
		}

		public override VectorElement BuildClusterElement(MapPos mapPos, VectorElementVector elements)
		{
			var popup = new BalloonPopup(
				mapPos,
				balloonPopupStyleBuilder.BuildStyle(),
				elements.Count.ToString(), "");

			return popup;
		}

	}
}

