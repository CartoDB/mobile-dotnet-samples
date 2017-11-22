
using System;
using System.IO;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using CoreGraphics;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS.Sections.VectorObjects
{
    public class City3DController : BaseController
    {
        DownloadBaseView contentView;

        string serverPath, localPath;
        MapPos position;

        HttpClientWithProgress client;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            contentView = new DownloadBaseView();
            View = contentView;
            contentView.AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

            MapPos chicago = new MapPos(-87.6298, 41.8781);
            position = contentView.Projection.FromWgs84(chicago);

            var baseUrl = "https://nutifront.s3.amazonaws.com/";
            var folder = "nml_models/";
            var filename = "chicago_tomtom_lod4_ios_2bpp.nmldb";

            serverPath = Path.Combine(baseUrl, folder);
            serverPath = Path.Combine(serverPath, filename);

            localPath = Utils.GetDocumentDirectory("cities_3d");

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            localPath = Path.Combine(localPath, filename);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            client = new HttpClientWithProgress(serverPath, localPath);

            StartDownload();

            client.ProgressChanged += OnProgressChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            client.Dispose();

            client.ProgressChanged -= OnProgressChanged;
        }

        void OnProgressChanged(long? size, long bytesDownloaded, double? progress)
        {
            int mb = (int)(size / 1024 / 1024);
            string text = $"DOWNLOADING CHICAGO ({mb}MB) {(int)progress}%";
            InvokeOnMainThread(delegate
            {
                if (progress.Equals(100.0))
                {
                    contentView.ProgressLabel.Hide();
                    AddCity(localPath);
                    ZoomToPosition();
                }
                else
                {
                    contentView.ProgressLabel.Update(text, (float)progress);
                }
            });
        }

        async void StartDownload()
        {
            await client.StartDownload();
        }

        void AddCity(string path)
        {
            var source = new OfflineNMLModelLODTreeDataSource(path);
            var layer = new NMLModelLODTreeLayer(source);

            layer.VisibleZoomRange = new MapRange(12, 24);
            contentView.MapView.Layers.Add(layer);
        }

        void ZoomToPosition()
        {
            contentView.MapView.SetFocusPos(position, 1);
            contentView.MapView.SetZoom(15, 1);
            contentView.MapView.SetTilt(30, 1);
        }
    }
}
