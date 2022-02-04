using System;
using System.Windows.Input;
using Xamarin.Forms;
using DGCValidator.Views;
using Xamarin.Essentials;
using DGCValidator.Resources;

namespace DGCValidator.ViewModels
{
    public class AboutViewModel
    {
        private Command backCommand;

        public AboutViewModel()
        {
     
        }
        public ICommand BackCommand => backCommand ??
        (backCommand = new Command(async () =>
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }));

        public String AppVersion
        {
            get { return AppResources.AppVersion + " " + VersionTracking.CurrentVersion; }
        }

        public String PublicKeyVersion
        {
            get { return AppResources.KeyVersion + " " + (App.CertificateManager.TrustList != null ? SecondsFromEpocToDateTime(App.CertificateManager.TrustList.Iat).ToString() : AppResources.MissingDataText); }
        }

        public String ValidationRulesVersion
        {
            get { return AppResources.ValidationRulesVersion + " " + App.CertificateManager.VaccinRules.ValueSetDate.Date; }
        }

        public static DateTime SecondsFromEpocToDateTime(long sec)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(sec).ToUniversalTime();
            return dtDateTime.ToLocalTime();
        }
    }
}
