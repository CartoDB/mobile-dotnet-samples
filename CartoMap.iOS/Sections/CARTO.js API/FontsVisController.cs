
using System;

namespace CartoMap.iOS
{
	public class FontsVisController : BaseVisController
	{
		public override string Name { get { return "Fonts Vis"; } }

		public override string Description { get { return "Vis displaying text on the map using UTFGrid"; } }

		protected override string Url
		{
			get
			{
                return "https://cartomobile-team.carto.com/u/nutiteq/api/v2/viz/13332848-27da-11e6-8801-0e5db1731f59/viz.json";
			}
		}
	}
}

