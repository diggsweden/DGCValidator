using System;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Views;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private TrustModel _trustModel;

        private ICommand refreshTrustListCommand;
        private Command backCommand;

        public SettingsViewModel()
        {
            Trust = new TrustModel{
                Issuer = App.CertificateManager.TrustList.Iss,
                IssuedDate = SecondsFromEpocToDateTime(App.CertificateManager.TrustList.Iat),
                ExpirationDate = SecondsFromEpocToDateTime(App.CertificateManager.TrustList.Exp)
            };
        }
        public ICommand BackCommand => backCommand ??
        (backCommand = new Command(async () =>
        {
         await App.Current.MainPage.Navigation.PopAsync();
        }));

        public ICommand RefreshTrustListCommand => refreshTrustListCommand ??
                (refreshTrustListCommand = new Command(async () =>
                {
                    App.CertificateManager.RefreshTrustListAsync();
                    App.CertificateManager.RefreshValueSetsAsync();
                }));

        public TrustModel Trust
        {
            get { return _trustModel; }
            set
            {
                _trustModel = value;
                OnPropertyChanged();
            }
        }

        public bool ProductionMode
        {
            get { return Xamarin.Essentials.Preferences.Get("ProductionMode", false); }
        }
        public static DateTime SecondsFromEpocToDateTime(long sec)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(sec).ToUniversalTime();
            return dtDateTime;
        }
        public bool NoVerificationMode
        {
            get { return Xamarin.Essentials.Preferences.Get("NoVerificationMode", false); }
        }

    }
}
