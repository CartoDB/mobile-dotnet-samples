
using System;

namespace CartoMobileSample
{
	public class SQLController : MapBaseController
	{
		public override string Name { get { return "SQL Map"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use Carto SQL API to get data" +
						 "and how to create custom VectorDataSource";
			}
		}

	}
}