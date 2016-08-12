
using System;
using Carto.PackageManager;
using CoreGraphics;
using UIKit;

namespace CartoMobileSample
{
	public class PackageListCell : UITableViewCell
	{
		public EventHandler<EventArgs> CellActionButtonClicked;

		UILabel nameLabel, statusLabel;

		PackageManagerButton ActionButton { get; set; }

		public PackageListCell()
		{
			nameLabel = new UILabel();
			nameLabel.Font = UIFont.FromName("Helvetica", 14);

			statusLabel = new UILabel();
			statusLabel.Font = UIFont.FromName("Helvetica", 12);

			ActionButton = new PackageManagerButton();

			AddSubviews(nameLabel, statusLabel, ActionButton);

			SelectionStyle = UITableViewCellSelectionStyle.None;

			ActionButton.TouchUpInside += OnButtonClick;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat padding = 5;

			nfloat totalWidth = Frame.Width - 4 * padding;

			nfloat nameWidth = totalWidth / 2.2f;
			nfloat statusWidth = totalWidth / 3;
			nfloat buttonWidth = totalWidth - (nameWidth + statusWidth);

			nfloat buttonHeight = Frame.Height / 2;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = nameWidth;
			nfloat h = Frame.Height;

			nameLabel.Frame = new CGRect(x, y, w, h);

			x += w + padding;
			w = statusWidth;

			statusLabel.Frame = new CGRect(x, y, w, h);

			x += w + padding;
			y = Frame.Height / 2 - buttonHeight / 2;
			w = buttonWidth;
			h = buttonHeight;

			ActionButton.Frame = new CGRect(x, y, w, h);
		}

		void OnButtonClick(object sender, EventArgs e)
		{
			if (CellActionButtonClicked != null)
			{
				CellActionButtonClicked(sender, e);
			}	
		}

		public void Update(Package pkg)
		{
			nameLabel.Text = pkg.Name;

			if (pkg.Info != null)
			{
				string status = "available";

				if (pkg.IsSmallerThan1MB)
				{
					status += " v." + pkg.Info.Version + " (<1MB)";
				}
				else {
					status += " v." + pkg.Info.Version + " (" + pkg.Info.Size.ToLong() / 1024 / 1024 + "MB)";
				}

				ActionButton.PackageId = pkg.Info.PackageId;

				// Check if the package is downloaded/is being downloaded (so that status is not null)
				if (pkg.Status != null)
				{
					if (pkg.Status.CurrentAction == PackageAction.PackageActionReady)
					{
						status = "ready";
						ActionButton.Text = "RM";
						ActionButton.Type = PackageManagerButtonType.StartRemovePackage;
					}
					else if (pkg.Status.CurrentAction == PackageAction.PackageActionWaiting)
					{
						status = "queued";
						ActionButton.Text = "C";
						ActionButton.Type = PackageManagerButtonType.CancelPackageTasks;
					}
					else {
						if (pkg.Status.CurrentAction == PackageAction.PackageActionCopying)
						{
							status = "copying";
						}
						else if (pkg.Status.CurrentAction == PackageAction.PackageActionDownloading)
						{
							status = "downloading";
						}
						else if (pkg.Status.CurrentAction == PackageAction.PackageActionRemoving)
						{
							status = "removing";
						}

						status += " " + ((int)pkg.Status.Progress).ToString() + "%";

						if (pkg.Status.Paused)
						{
							status = status + " (paused)";
							ActionButton.Text = "R";
							ActionButton.Type = PackageManagerButtonType.SetPackagePriority;
							ActionButton.PriorityIndex = 0;
						}
						else {
							ActionButton.Text = "P";
							ActionButton.Type = PackageManagerButtonType.SetPackagePriority;
							ActionButton.PriorityIndex = -1;
						}
					}
				}
				else {
					ActionButton.Text = "DL";
					ActionButton.Type = PackageManagerButtonType.StartPackageDownload;
				}

				statusLabel.Text = status;
			}
			else {
				ActionButton.Text = ">";
				ActionButton.Type = PackageManagerButtonType.UpdatePackages;
				ActionButton.PackageName = pkg.Name;
				statusLabel.Text = "";
			}
		}
	}
}

