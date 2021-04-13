using System;
using ZXing.Mobile;
using Xamarin.Forms;
using DGCValidator.Services;
using DGCValidator.Resources;
using System.Threading.Tasks;
using System.Collections.Generic;

[assembly: Dependency(typeof(DGCValidator.iOS.Services.QRScanningService))]

namespace DGCValidator.iOS.Services
{
    public class QRScanningService : IQRScanningService
    {
        public QRScanningService()
        {
        }

        public async Task<String> ScanAsync()
        {
            var optionsCustom = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<ZXing.BarcodeFormat>() {
                    ZXing.BarcodeFormat.AZTEC,
                    ZXing.BarcodeFormat.QR_CODE
                },
                UseNativeScanning=true,
                AutoRotate = true
            };
            
            var scanner = new MobileBarcodeScanner()
            {
                TopText = AppResources.ScanTopText,
                BottomText = AppResources.ScanBottomText,
                FlashButtonText = AppResources.ScanFlashText,
                CancelButtonText = AppResources.ScanCancelText,
            };

            try
            {
                var scanResult = await scanner.Scan(optionsCustom,true);
                if (scanResult != null)
                {
                    return scanResult.Text;
                }
            }catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
