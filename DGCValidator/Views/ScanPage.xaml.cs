using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                // Turn On Flashlight  
                await Flashlight.TurnOnAsync();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await ShowAlert(fnsEx.Message);
            }
            catch (PermissionException pEx)
            {
                await ShowAlert(pEx.Message);
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
            }
        }

        async void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            try
            {
                // Turn Off Flashlight  
                await Flashlight.TurnOffAsync();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await ShowAlert(fnsEx.Message);
            }
            catch (PermissionException pEx)
            {
                await ShowAlert(pEx.Message);
            }
            catch (Exception ex)
            {
                await ShowAlert(ex.Message);
            }
        }

        public async Task ShowAlert(string message)
        {
            await DisplayAlert("Faild", message, "Ok");
        }
    }
}
