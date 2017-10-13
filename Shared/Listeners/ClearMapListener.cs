using System;
using Carto.DataSources;
using Carto.Ui;

namespace Shared.Listeners
{
    public class ClearMapListener : MapEventListener
    {
        public LocalVectorDataSource Source { get; set; }

        public override void OnMapClicked(MapClickInfo mapClickInfo)
        {
            Source.Clear();
        }
    }
}
