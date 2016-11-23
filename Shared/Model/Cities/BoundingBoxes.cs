
using System.Collections.Generic;

namespace Shared
{
	public class BoundingBoxes
	{
		public List<BoundingBox> List
		{
			get
			{
				List<BoundingBox> list = new List<BoundingBox>();

				var berlin = new BoundingBox { Name = "Berlin", MinLon = 13.2285, MaxLon = 13.5046, MinLat = 52.4698, MaxLat = 52.57477 };
				list.Add(berlin);

				// New York City|{"north":41.0043135940002,"south":40.46218192,"east":-73.476830356999926,"west":-74.120569522937387}
				var newYork = new BoundingBox { Name = "New York", MinLon = -73.4768, MaxLon = -74.1205, MinLat = 40.4621, MaxLat = 41.0043 };
				list.Add(newYork);

				// Madrid|{"north":40.490459253099083,"south":40.3825376603664,"east":-3.6432195181857878,"west":-3.742797695059835}
				var madrid = new BoundingBox { Name = "Madrid", MinLon = -3.7427, MaxLon = -3.6432, MinLat = 40.3825, MaxLat = 40.4904 };
				list.Add(madrid);

				// Paris|{"north":48.9176506405876,"south":48.808907900386004,"east":2.4356643938165234,"west":2.1814194876997135}
				var paris = new BoundingBox { Name = "Paris", MinLon = 2.1814, MaxLon = 2.4356, MinLat = 48.8089, MaxLat = 48.9176 };
				list.Add(paris);

				// San Francisco|{"north":37.856965020544393,"south":37.602264255657843,"east":-122.33688354492188,"west":-122.56210327148436}
				var sanFrancisco = new BoundingBox { Name = "San Francisco", MinLon = -122.3368, MaxLon = -122.56210, MinLat = 37.6022, MaxLat = 37.6022 };
				list.Add(sanFrancisco);

				// London|{"north":51.6939004971554,"south":51.2871004967765,"east":0.32760048286911569,"west":-0.503699517905261}
				var london = new BoundingBox { Name = "London", MinLon = -0.5036, MaxLon = 0.3276, MinLat = 51.2871, MaxLat = 51.6939 };
				list.Add(london);

				var tartu = new BoundingBox { Name = "Tartu", MinLon = 26.6548, MaxLon = 26.7901, MinLat = 58.3404, MaxLat = 58.3964 };
				list.Add(tartu);

				var newDelhi = new BoundingBox { Name = "New Delhi", MinLon = 77.1477, MaxLon = 77.2757, MinLat = 28.5361, MaxLat = 28.6368 };
				list.Add(newDelhi);

				return list;
			}
		}
	}
}
