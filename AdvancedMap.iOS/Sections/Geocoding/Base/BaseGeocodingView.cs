
using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Geocoding;
using Carto.Geometry;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
    public class BaseGeocodingView : UIView
    {
        public MapView MapView { get; private set; }

        public SlideInPopup Popup { get; private set; }

        LocalVectorDataSource source;

        PopupButton PackageButton;

        Projection Projection
        {
            get { return MapView.Options.BaseProjection; }
        }

        public PackagePopupContent PackageContent { get; private set; }

        public ProgressLabel ProgressLabel { get; private set; }

        public BaseGeocodingView()
        {
            MapView = new MapView();
            AddSubview(MapView);

            var layer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            MapView.Layers.Add(layer);

            source = new LocalVectorDataSource(Projection);
            var objectLayer = new VectorLayer(source);
            MapView.Layers.Add(objectLayer);

            PackageButton = new PopupButton("icons/icon_global.png");
            AddButton(PackageButton);

            PackageContent = new PackagePopupContent();

            PackageButton.AddGestureRecognizer(new UITapGestureRecognizer(PackageButtonTapped));

            Popup = new SlideInPopup();
            AddSubview(Popup);
            SendSubviewToBack(Popup);

            ProgressLabel = new ProgressLabel();
            AddSubview(ProgressLabel);
        }

        void PackageButtonTapped()
        {
            Popup.Header.SetText("SELECT A PACKAGE TO DOWNLOAD");
            Popup.SetContent(PackageContent);
            Popup.Show();
        }

        nfloat bottomLabelHeight = 40;
        nfloat smallPadding = 5;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            MapView.Frame = Bounds;
            Popup.Frame = Bounds;

            int count = buttons.Count;

            nfloat buttonWidth = 60;
            nfloat innerPadding = 25;
            nfloat totalArea = buttonWidth * count + (innerPadding * (count - 1));

            var w = buttonWidth;
            var h = w;
            var y = Frame.Height - (bottomLabelHeight + h + smallPadding);
            var x = Frame.Width / 2 - totalArea / 2;

            foreach (PopupButton button in buttons)
            {
                button.Frame = new CGRect(x, y, w, h);
                x += w + innerPadding;
            }

            w = Frame.Width;
            h = bottomLabelHeight;
            x = 0;
            y = Frame.Height - h;

            ProgressLabel.Frame = new CGRect(x, y, w, h);
        }

        readonly List<PopupButton> buttons = new List<PopupButton>();

        public void AddButton(PopupButton button)
        {
            buttons.Add(button);
            AddSubview(button);
        }

        public string Folder { get; set; } = "";

        public void UpdatePackages(List<Package> packages)
        {
            PackageContent.AddPackages(packages);
        }

        public void UpdateFolder(Package package)
        {
            Folder += package.Name + "/";
        }
    }
}
