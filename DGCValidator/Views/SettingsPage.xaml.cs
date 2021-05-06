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
        }

        private void OnToggled(object sender, ToggledEventArgs e)
        {
            Xamarin.Essentials.Preferences.Set("ProductionMode", e.Value);
        }
    }
}
