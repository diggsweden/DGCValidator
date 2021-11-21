using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing;

namespace DGCValidator.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();
			_scanView.Options = opts;
		}

		public static readonly ZXing.Mobile.MobileBarcodeScanningOptions opts = new ZXing.Mobile.MobileBarcodeScanningOptions
		{
			PossibleFormats = new List<ZXing.BarcodeFormat>() {
					ZXing.BarcodeFormat.AZTEC,
					ZXing.BarcodeFormat.QR_CODE
				},
			UseNativeScanning = true,
			AutoRotate = true
		};

		protected override void OnAppearing()
		{
			base.OnAppearing();
			_scanView.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			//_scanView.IsScanning = false;
		}
	}
}
