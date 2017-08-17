using Carto.Ui;
using Carto.Layers;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Styles;
using System.Linq;
using Carto.Core;
using Carto.Graphics;

namespace Shared
{
	public class VectorElementListener : VectorElementEventListener
	{
        public const string NullString = "null";

        public const string RouteSearchTitle = "route_search_title";
        public const string RouteSearchDescription = "route_search_description";

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
            builder.CornerRadius = 5;
            builder.TitleFontSize = 12;
            builder.DescriptionFontSize = 10;

            var navy = new Color(22, 41, 69, 255);
            builder.TitleColor = navy;
            builder.DescriptionColor = navy;

            var animationBuilder = new AnimationStyleBuilder();
            animationBuilder.RelativeSpeed = 2.0f;
            animationBuilder.SizeAnimationType = AnimationType.AnimationTypeSpring;
            builder.AnimationStyle = animationBuilder.BuildStyle();

			BalloonPopupStyle style = builder.BuildStyle();

			string title = element.GetMetaDataElement("ClickText").String;
			string description = "";

            if (!element.GetMetaDataElement(RouteSearchTitle).String.Equals(NullString))
            {
                // Route search has a special click text
                title = element.GetMetaDataElement(RouteSearchTitle).String;
                description = element.GetMetaDataElement(RouteSearchDescription).String;
            }
            else
            {
				for (int i = 0; i < element.MetaData.Count; i++)
				{
					string key = element.MetaData.Keys.ToList()[i];
					description += key + " = " + element.GetMetaDataElement(key) + "; ";
				}    
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

