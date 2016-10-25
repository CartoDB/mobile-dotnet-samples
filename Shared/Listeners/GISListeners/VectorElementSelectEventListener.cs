using System;
using Carto.Layers;
using Carto.Ui;

namespace Shared
{
	public class VectorElementSelectEventListener : VectorElementEventListener
	{
		EditableVectorLayer vectorLayer;

		public VectorElementSelectEventListener(EditableVectorLayer vectorLayer)
		{
			this.vectorLayer = vectorLayer;
		}

		public override bool OnVectorElementClicked(VectorElementClickInfo clickInfo)
		{
			vectorLayer.SelectedVectorElement = clickInfo.VectorElement;
			return true;
		}
	}

}

