using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DGCValidator.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        private async void NavigateButton_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
        protected void VerifierFaq(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.digg.se/utveckling-av-digital-forvaltning/verifieringslosning-for-vaccinationsbevis"));
        }
    }
}
