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
        protected void VerifierFaq(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://digg.se"));
        }
    }
}
