using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DGCValidator.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void OnProductionToggled(object sender, ToggledEventArgs e)
        {
            Xamarin.Essentials.Preferences.Set("ProductionMode", e.Value);
        }
        private void OnNoVerificationToggled(object sender, ToggledEventArgs e)
        {
            Xamarin.Essentials.Preferences.Set("NoVerificationMode", e.Value);
        }

    }
}
