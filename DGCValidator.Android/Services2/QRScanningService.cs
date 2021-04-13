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
using QRScanner.Services;

[assembly: Dependency(typeof(QRScanner.Droid.Services.QRScanningService))]

namespace QRScanner.Droid.Services
{
    public class QRScanningService : IQRScanningService
    {
        public async Task<ProofCode> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Skanna vaccinationsbeviset",
                BottomText = "",
            };

            var scanResult = await scanner.Scan(optionsCustom);
            if (scanResult != null)
            {
                return CryptoService.DecryptData(scanResult.Text);
            }
            return null;
        }
    }
}