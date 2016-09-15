using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;

using Carto.Ui;
using Carto.VectorElements;
using System;
using Windows.System.Threading;

namespace HelloMap.WindowsPhone
{
    public static class MapExtensions
    {
        public static async void UpdateVis(this MapView map, string url, Action<string> error = null)
        {
            await ThreadPool.RunAsync(delegate
            {
                map.Layers.Clear();

                // Create VIS loader
                CartoVisLoader loader = new CartoVisLoader();
                loader.DefaultVectorLayerMode = true;
                BasicCartoVisBuilder builder = new BasicCartoVisBuilder(map);

                try
                {
                    loader.LoadVis(builder, url);
                }
                catch (Exception e)
                {
                    if (error != null)
                    {
                        error(e.Message);
                    }
                }
            });
        }

        public static async void AddMarkerToPosition(this MapView map, MapPos position)
        {
            await ThreadPool.RunAsync(delegate
            {
                // Initialize a local vector data source
                Projection projection = map.Options.BaseProjection;
                LocalVectorDataSource datasource = new LocalVectorDataSource(projection);

                // Initialize a vector layer with the previous data source
                VectorLayer layer = new VectorLayer(datasource);

                // Add layer to map
                map.Layers.Add(layer);

                MarkerStyleBuilder builder = new MarkerStyleBuilder();
                builder.Size = 15;
                builder.Color = new Carto.Graphics.Color(0, 255, 0, 255);
                
                MarkerStyle style = builder.BuildStyle();

                // Create marker and add it to the source
                Marker marker = new Marker(position, style);
                datasource.Add(marker);
            });
        }
    }
}
