
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class BasicEditableOverlayController : MapBaseController
	{
		public override string Name { get { return "Basic editable overlay"; } }

		public override string Description { get { return "Shows usage of an editable vector layer"; } }

		LocalVectorDataSource source;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Initialize source and Edit layer, add it to the map
			source = new LocalVectorDataSource(MapView.Options.BaseProjection);

			EditableVectorLayer editLayer = new EditableVectorLayer(source);
			MapView.Layers.Add(editLayer);

			// Convenience methods to add elements to the map, cf. LocalVectorDataSourceExtensions
			source.AddPoint(new MapPos(-5000000, -900000));

			source.AddLine(new MapPosVector {
				new MapPos(-6000000, -500000), new MapPos(-9000000, -500000)
			});

			source.AddPolygon(new MapPosVector {
				new MapPos(-5000000, -5000000), new MapPos(5000000, -5000000), new MapPos(0, 10000000)
			});

			// Add a vector element even listener to select elements (on element click)
			editLayer.VectorElementEventListener = new VectorElementSelectEventListener(editLayer);

			// Add a map even listener to deselect element (on map click)
			MapView.MapEventListener = new VectorElementDeselectEventListener(editLayer);

			// Add the vector element edit even listener
			editLayer.VectorEditEventListener = new BasicEditEventListener(source);
		}
	}
}

