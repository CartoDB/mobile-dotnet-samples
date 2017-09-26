
using System;
using Foundation;
using UIKit;
using System.Collections.Generic;
using Shared.Model;

namespace AdvancedMap.iOS.Sections.BaseMap.Subviews
{
    public class LanguagePopupContent : UIView, IUITableViewDataSource
    {
        const string Identifier = "LanguageCell";

        public List<Language> Languages { get; private set; }

        public UITableView Table { get; private set; }

        public LanguagePopupContent()
        {
            Table = new UITableView();
            Table.DataSource = this;
            Table.RegisterClassForCellReuse(typeof(UITableViewCell), Identifier);
            AddSubview(Table);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Table.Frame = Bounds;
        }

        public void AddLanguages(List<Language> languages)
        {
            Languages = languages;
            Table.ReloadData();
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(Identifier, indexPath);
            cell.TextLabel.Text = Languages[indexPath.Row].Name;

            return cell;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return Languages.Count;
        }
    }
}

