using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DGCValidator.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected void VerifierFaq(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://www.digg.se/utveckling-av-digital-forvaltning/verifieringslosning-for-vaccinationsbevis"));
        }
        protected void PrivacyPolicyLink(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://www.digg.se/vaccinationsbevis-verifiering/integritetspolicy"));
        }
    }
}
