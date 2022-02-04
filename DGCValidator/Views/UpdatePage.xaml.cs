using System;
using System.Collections.Generic;
using System.Globalization;
using DGCValidator.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DGCValidator.Views
{
    public partial class UpdatePage : ContentPage
    {


        public UpdatePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            App.UpdatePageStore.UpdatePageVisable = false;
            return false; 
        }

        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            string url = string.Empty;
            var location = RegionInfo.CurrentRegion.Name.ToLower();
            if (Device.RuntimePlatform == Device.Android)
                url = "https://play.google.com/store/apps/details?id=se.digg.dccvalidator";
            else
                url = "https://apps.apple.com/" + location + "/app/vaccinationsbevis-verifiering/id1597745749";
            await Browser.OpenAsync(url, BrowserLaunchMode.External);


        }
    }
}
