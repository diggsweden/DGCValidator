using System;
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using System.Threading.Tasks;  
using Android.App;  
using Android.Content;  
using Android.OS;  
using Android.Runtime;  
using Android.Views;  
using Android.Widget;
using ZXing.Mobile;
using Xamarin.Forms;
using DGCValidator.Services;
using DGCValidator.Resources;
using Application = Android.App.Application;

[assembly: Dependency(typeof(DGCValidator.Droid.Services.QRScanningService))]

namespace DGCValidator.Droid.Services
{
    public class QRScanningService : IQRScanningService
    {
        public async Task<String> ScanAsync()
        {
            var optionsCustom = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat>() {
                    ZXing.BarcodeFormat.QR_CODE
                },
                UseNativeScanning = true,
                TryHarder = false,
                AutoRotate = true
                //CameraResolutionSelector = CameraResolutionSelector.SelectLowestResolutionMatchingDisplayAspectRatio
            };
        

            var scanner = new MobileBarcodeScanner()
            {
                TopText = AppResources.ScanTopText,
                BottomText = AppResources.ScanBottomText,
                FlashButtonText = AppResources.ScanFlashText,
                CancelButtonText = AppResources.ScanCancelText,
            };

            scanner.UseCustomOverlay = true;
            var customOverlay = new ScannerView(Application.Context, AppResources.ScanTopText, scanner);
            scanner.CustomOverlay = customOverlay;
            try
            {
                var scanResult = await scanner.Scan(optionsCustom);
                if (scanResult != null)
                {
                    return scanResult.Text;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}