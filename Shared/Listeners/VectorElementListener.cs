using Carto.Ui;
using Carto.Layers;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Styles;
using Carto.Utils;
using System.Linq;
using Carto.Core;

namespace Shared
{
	public class VectorElementListener : VectorElementEventListener
	{
		LocalVectorDataSource source;

		BalloonPopup previous;

		public VectorElementListener(LocalVectorDataSource dataSource)
		{
			source = dataSource;
		}

		public override bool OnVectorElementClicked(VectorElementClickInfo clickInfo)
		{
			if (previous != null)
			{
				source.Remove(previous);
			}

			VectorElement element = clickInfo.VectorElement;

			BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();
			builder.LeftMargins = new BalloonPopupMargins(0, 0, 0, 0);
			builder.RightMargins = new BalloonPopupMargins(6, 3, 6, 3);
			builder.PlacementPriority = 10;

			BalloonPopupStyle style = builder.BuildStyle();

			string title = element.GetMetaDataElement("ClickText").String;
			string description = "";

			for (int i = 0; i < element.MetaData.Count; i++)
			{
				string key = element.MetaData.Keys.ToList()[i];
				description += key + " = " + element.GetMetaDataElement(key) + "; ";
			}

			BalloonPopup popup;

			if (element is BalloonPopup)
			{
				Billboard billboard = (Billboard)element;
				popup = new BalloonPopup(billboard, style, title, description);
			}
			else
			{
				MapPos position = clickInfo.ClickPos;
				popup = new BalloonPopup(position, style, title, description);
			}

			source.Add(popup);
			previous = popup;

			return true;
		}
	}
}

