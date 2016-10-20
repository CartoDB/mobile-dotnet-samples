using System;
namespace CartoMap.iOS
{
	public class DotsVisController : BaseVisController
	{
		public override string Name { get { return "Dots Vis"; } }

		public override string Description { get { return "Vis showing dots on the map"; } }

		protected override string Url
		{
			get
			{
				return "https://documentation.cartodb.com/api/v2/viz/236085de-ea08-11e2-958c-5404a6a683d5/viz.json";
			}
		}
	}
}

