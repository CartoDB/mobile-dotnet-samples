
using System;
using System.IO;
using Android.App;
using Android.OS;
using Android.Views;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.VectorObject
{
    [Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class City3DActivity : BaseActivity
    {
        DownloadBaseView contentView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            contentView = new DownloadBaseView(this,
                                               Resource.Drawable.icon_info_blue,
                                               Resource.Drawable.icon_back_blue,
                                               Resource.Drawable.icon_close,
                                               Resource.Drawable.icon_wifi_on,
                                               Resource.Drawable.icon_wifi_off,
                                               Resource.Drawable.icon_info_white);
            contentView.SetFrame();
            contentView.OnlineSwitch.Visibility = ViewStates.Gone;

            SetContentView(contentView);

            contentView.AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

            MapPos chicago = new MapPos(-87.6298, 41.8781);
            MapPos position = contentView.Projection.FromWgs84(chicago);

            var baseUrl = "https://nutifront.s3.amazonaws.com/";
            var folder = "nml_models/";
            var filename = "chicago_tomtom_lod4_ios_2bpp.nmldb";

            string serverPath = Path.Combine(baseUrl, folder);
            serverPath = Path.Combine(serverPath, filename);

            string directory = GetExternalFilesDir(null).ToString();
            string localPath = Path.Combine(directory, "cities_3d");

            if (!Directory.Exists(localPath))
            {
                Directory.CreateDirectory(localPath);
            }

            localPath = Path.Combine(localPath, filename);

            //if (File.Exists(localPath))
            //{
            //    AddCity(localPath);
            //    ZoomToPosition(position);
            //}
            //else
            //{
                DownloadCity(serverPath, localPath, position);
            //}
        }

        async void DownloadCity(string serverPath, string localPath, MapPos position)
        {
            using (var client = new HttpClientWithProgress(serverPath, localPath))
            {
                client.ProgressChanged += (size, bytesDownloaded, progress) =>
                {
                    int mb = (int)(size / 1024 / 1024);
                    string text = $"DOWNLOADING CHICAGO ({mb}MB) {(int)progress}%";
                    RunOnUiThread(delegate
                    {
                        if (progress.Equals(100.0))
                        {
                            contentView.ProgressLabel.Hide();
                            AddCity(localPath);
                            ZoomToPosition(position);
                        }
                        else
                        {
                            contentView.ProgressLabel.Update(text, (float)progress);
                        }

                    });
                };

                await client.StartDownload();
            }
        }

        void AddCity(string path)
        {
            var source = new OfflineNMLModelLODTreeDataSource(path);
            var layer = new NMLModelLODTreeLayer(source);

            layer.VisibleZoomRange = new MapRange(12, 24);
            contentView.MapView.Layers.Add(layer);
        }

        void ZoomToPosition(MapPos position)
        {
            contentView.MapView.SetFocusPos(position, 1);
            contentView.MapView.SetZoom(15, 1);
            contentView.MapView.SetTilt(30, 1);
        }
    }
}
