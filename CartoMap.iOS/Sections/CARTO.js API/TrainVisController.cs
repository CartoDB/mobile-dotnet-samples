using System;
namespace CartoMap.iOS
{
	public class TrainVisController : BaseVisController
	{
		public override string Name { get { return "NYCity Subway Vis"; } }

		public override string Description { get { return "Vis displaying thes subway in different colors using UTFGrid"; } }

		protected override string Url
		{
			get
			{
				return "https://mamataakella.cartodb.com/api/v2/viz/30730478-bbb5-11e5-b75c-0e5db1731f59/viz.json";
			}
		}
	}
}

