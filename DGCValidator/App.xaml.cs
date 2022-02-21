using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DGCValidator.Views;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services;
using Xamarin.Essentials;

namespace DGCValidator
{
    public partial class App : Application
    {
        public static CertificateManager CertificateManager { get; private set; }
        public static string _latestVersion { get; set; }
        public App()
        {
            InitializeComponent();
            CertificateManager = new CertificateManager(new RestService());
            MainPage = new NavigationPage(new MainPage()) { BarBackgroundColor = Color.White };
            Xamarin.Essentials.Preferences.Set("NoVerificationMode", false);
            Xamarin.Essentials.Preferences.Set("ProductionMode", false);
        }

        protected override async void OnStart()
        {
            CertificateManager.LoadCertificates();
            CertificateManager.LoadValueSets();
            await CertificateManager.LoadVaccineRules();

            await EnsureUpdatedVersion();
        }

        protected override void OnSleep()
        {
        }
        protected override async void OnResume()
        {
            CertificateManager.LoadCertificates();
            CertificateManager.LoadValueSets();
            await CertificateManager.LoadVaccineRules();

            await EnsureUpdatedVersion();
        }


        private async Task EnsureUpdatedVersion()
        {
            var deviceVersion = AppInfo.Version;
            _latestVersion = Device.RuntimePlatform switch
            {
                Device.Android => CertificateManager.VaccinRules.AppVersion.Android,
                Device.iOS => CertificateManager.VaccinRules.AppVersion.IOS,
                _ => null
            };

            if (_latestVersion == null) return;

            var latestVersionArray = _latestVersion.Split('.');
            var latestMajorVersion = int.Parse(latestVersionArray[0]);
            var latestMinorVersion = int.Parse(latestVersionArray[1]);

            if ((latestMajorVersion > deviceVersion.Major || latestMinorVersion > deviceVersion.Minor) && !UpdatePageStore.UpdatePageVisable)
            {
                UpdatePageStore.UpdatePageVisable = true;
                await Current.MainPage.Navigation.PushAsync(new UpdatePage());
            }
        }
        public static class UpdatePageStore
        {
            public static bool UpdatePageVisable { get; internal set; }
        }
    }
}
