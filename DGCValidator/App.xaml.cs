using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DGCValidator.Views;
using DGCValidator.Services.CWT.Certificates;
using DGCValidator.Services;

namespace DGCValidator
{
    public partial class App : Application
    {
        public static CertificateManager CertificateManager { get; private set; }

        public App()
        {
            InitializeComponent();
            CertificateManager = new CertificateManager(new RestService());
            MainPage = new NavigationPage(new MainPage { });
        }

        protected override void OnStart()
        {
            CertificateManager.LoadCertificates();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
