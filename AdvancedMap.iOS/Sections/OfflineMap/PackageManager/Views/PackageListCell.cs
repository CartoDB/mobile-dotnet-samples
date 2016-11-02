
using System;
using Carto.PackageManager;
using CoreGraphics;
using Shared;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PackageListCell : UITableViewCell
	{
		public EventHandler<EventArgs> CellActionButtonClicked;

		UILabel nameLabel, statusLabel;

		PackageManagerButton Button { get; set; }

		static nfloat MaxButtonWidth = 70;

		Package package;

		public PackageListCell()
		{
			nameLabel = new UILabel();
			nameLabel.Font = UIFont.FromName("HelveticaNeue-Bold", 15);

			statusLabel = new UILabel();
			statusLabel.Font = UIFont.FromName("HelveticaNeue", 12);
			statusLabel.TextColor = UIColor.DarkGray;

			Button = new PackageManagerButton();
			Button.TitleLabel.TextAlignment = UITextAlignment.Center;

			AddSubviews(nameLabel, statusLabel, Button);

			SelectionStyle = UITableViewCellSelectionStyle.None;

			Button.TouchUpInside += OnButtonClick;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (package == null)
			{
				return;
			}

			nfloat padding = 5;

			nfloat totalWidth = Frame.Width - 3 * padding;

			nfloat textWidth = totalWidth * 0.7f;
			nfloat buttonWidth = totalWidth - textWidth;

			nfloat buttonHeight = Frame.Height / 2;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = textWidth;
			// If package has info (subtitle), make room for it, else full height
			nfloat h = package.HasInfo() ? Frame.Height / 3 * 2 : Frame.Height;

			nameLabel.Frame = new CGRect(x, y, w, h);

			y += h * 0.75f;
			h = Frame.Height - h;

			statusLabel.Frame = new CGRect(x, y, w, h);

			if (buttonWidth > MaxButtonWidth) {
				buttonWidth = MaxButtonWidth;
			}

			x = totalWidth - (buttonWidth + padding);
			y = Frame.Height / 2 - buttonHeight / 2;
			w = buttonWidth;
			h = buttonHeight;

			Button.Frame = new CGRect(x, y, w, h);
		}

		void OnButtonClick(object sender, EventArgs e)
		{
			if (CellActionButtonClicked != null)
			{
				CellActionButtonClicked(sender, e);
			}	
		}

		public void Update(Package package)
		{
			this.package = package;

			nameLabel.Text = package.Name;

			if (package.Info != null)
			{
				string status = "available";

				if (package.IsSmallerThan1MB)
				{
					status += " v." + package.Info.Version + " (<1MB)";
				}
				else {
					status += " v." + package.Info.Version + " (" + package.Info.Size.ToLong() / 1024 / 1024 + "MB)";
				}

				Button.PackageId = package.Info.PackageId;

				// Check if the package is downloaded/is being downloaded (so that status is not null)
				if (package.Status != null)
				{
					if (package.Status.CurrentAction == PackageAction.PackageActionReady)
					{
						status = "ready";
						Button.Text = "Remove";
						Button.Type = PMButtonType.StartRemovePackage;
					}
					else if (package.Status.CurrentAction == PackageAction.PackageActionWaiting)
					{
						status = "queued";
						Button.Text = "Cancel";
						Button.Type = PMButtonType.CancelPackageTasks;
					}
					else {
						if (package.Status.CurrentAction == PackageAction.PackageActionCopying)
						{
							status = "copying";
						}
						else if (package.Status.CurrentAction == PackageAction.PackageActionDownloading)
						{
							status = "downloading";
						}
						else if (package.Status.CurrentAction == PackageAction.PackageActionRemoving)
						{
							status = "removing";
						}

						status += " " + ((int)package.Status.Progress).ToString() + "%";

						if (package.Status.Paused)
						{
							status = status + " (paused)";
							Button.Text = "Resume";
							Button.Type = PMButtonType.SetPackagePriority;
							Button.PriorityIndex = 0;
						}
						else {
							Button.Text = "Pause";
							Button.Type = PMButtonType.SetPackagePriority;
							Button.PriorityIndex = -1;
						}
					}
				}
				else {
					Button.Text = "Download";
					Button.Type = PMButtonType.StartPackageDownload;
				}

				statusLabel.Text = status;
				Button.Font = UIFont.FromName("HelveticaNeue-Light", 12);
			}
			else {
				Button.Font = UIFont.FromName("HelveticaNeue-Bold", 14);
				Button.Text = ">";
				Button.Type = PMButtonType.UpdatePackages;
				Button.PackageName = package.Name;
				statusLabel.Text = "";
			}
		}
	}
}

