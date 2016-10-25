using System;
using Carto.Layers;
using Carto.Ui;

namespace Shared
{
	public class VectorElementDeselectEventListener : MapEventListener
	{
		EditableVectorLayer vectorLayer;

		public VectorElementDeselectEventListener(EditableVectorLayer vectorLayer)
		{
			this.vectorLayer = vectorLayer;
		}

		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			vectorLayer.SelectedVectorElement = null;
		}
	}
}

