using System;
using CoreGraphics;
using UIKit;

namespace DGCValidator.iOS.Services
{
    public class ScannerView : UIView
    {
		private UIButton torchButton;
		private UIButton cancelButton;
		private UIView topBg;
		private UIView bottomBg;
		private UILabel scanTextLabel;
		readonly string scanText;
		readonly string cancelText;

		public ScannerView(string scanText, string cancelText) : base()
		{
			this.scanText = scanText ?? "";
			this.cancelText = cancelText ?? "Avbryt";

			Initialize();
		}

		private void Initialize()
        {
			Opaque = false;
			BackgroundColor = UIColor.Clear;

			var picFrameWidth = Math.Round(Frame.Width * 0.90);
			var picFrameHeight = Math.Round(Frame.Height * 0.90);
			var picFrameX = (Frame.Width - picFrameWidth) / 2;
			var picFrameY = (Frame.Height - picFrameHeight) / 2;

			var picFrame = new CGRect((int)picFrameX, (int)picFrameY, (int)picFrameWidth, (int)picFrameHeight);

			//Setup Overlay
			var overlaySize = new CGSize(Frame.Width, Frame.Height);

			topBg = new UIView(new CGRect(0, 0, overlaySize.Width, (overlaySize.Height - picFrame.Height) / 2));
			topBg.Frame = new CGRect(0, 0, overlaySize.Width, overlaySize.Height * 0.30f);
			topBg.BackgroundColor = UIColor.Black;
			topBg.Alpha = 0.6f;
			topBg.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin;
			AddSubview(topBg);

			bottomBg = new UIView(new CGRect(0, topBg.Frame.Height + picFrame.Height, overlaySize.Width, topBg.Frame.Height));
			bottomBg.Frame = new CGRect(0, overlaySize.Height * 0.70f, overlaySize.Width, overlaySize.Height * 0.30f);
			bottomBg.BackgroundColor = UIColor.Black;
			bottomBg.Alpha = 0.6f;
			bottomBg.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin;
			AddSubview(bottomBg);

			scanTextLabel = new UILabel()
			{
				Frame = topBg.Frame,
				Text = scanText,
				Font = UIFont.SystemFontOfSize(35),
				TextAlignment = UITextAlignment.Center,
				TextColor = UIColor.White,
				Lines = 0,
				BackgroundColor = UIColor.Clear
			};

			scanTextLabel.SizeToFit();
			AddSubview(scanTextLabel);

			torchButton = new UIButton(UIButtonType.RoundedRect);
			torchButton.SetImage(UIImage.FromBundle("flash.png"), UIControlState.Normal);
			torchButton.SetBackgroundImage(UIImage.FromBundle("flash.png"), UIControlState.Normal);
			torchButton.TintColor = UIColor.Clear;
			torchButton.Frame = new CGRect(overlaySize.Width/2-30, overlaySize.Height * 0.75f, 60, 60);
			torchButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin;
			AddSubview(torchButton);

			cancelButton = UIButton.FromType(UIButtonType.RoundedRect);
//			cancelButton.BackgroundColor = UIColor.Clear;
//			cancelButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			cancelButton.SetImage(UIImage.FromBundle("avbryt.png"), UIControlState.Normal);
			cancelButton.SetBackgroundImage(UIImage.FromBundle("avbryt.png"), UIControlState.Normal);
			cancelButton.TintColor = UIColor.Clear;
			cancelButton.SetTitle(cancelText, UIControlState.Normal);
			cancelButton.Frame = new CGRect(overlaySize.Width / 2 - 30, overlaySize.Height * 0.85f, 60, 60);
			cancelButton.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleLeftMargin;

//			cancelButton.TitleEdgeInsets = new UIEdgeInsets(0, 40, 0, 0);
//			cancelButton.ImageEdgeInsets = new UIEdgeInsets(0, 0, 0, 60);
			AddSubview(cancelButton);
		}
		public override void LayoutSubviews()
		{
			var overlaySize = new CGSize(Frame.Width, Frame.Height);
			if (topBg != null)
				topBg.Frame = new CGRect(0, 0, overlaySize.Width, overlaySize.Height * 0.30f);
			if (bottomBg != null)
				bottomBg.Frame = new CGRect(0, overlaySize.Height * 0.70f, overlaySize.Width, overlaySize.Height * 0.30f);
			if (scanTextLabel != null)
				scanTextLabel.Frame = topBg.Frame;

			torchButton.Frame = new CGRect(overlaySize.Width / 2 - 30, overlaySize.Height * 0.75f, 60, 60);
			cancelButton.Frame = new CGRect(overlaySize.Width / 2 - 30, overlaySize.Height * 0.85f, 60, 60);
		}

		public void Destroy()
			=> InvokeOnMainThread(() =>
			{
				scanTextLabel.RemoveFromSuperview();
				topBg.RemoveFromSuperview();
				bottomBg.RemoveFromSuperview();
				torchButton.RemoveFromSuperview();
				cancelButton.RemoveFromSuperview();
				
				scanTextLabel = null;
				topBg = null;
				bottomBg = null;
				torchButton = null;
				cancelButton = null;
			});

		public UIButton TorchButton
		{
			get { return torchButton; }
		}
		public UIButton CancelButton
		{
			get { return cancelButton; }
		}
	}
}
