using System;
using System.Windows.Input;
using Xamarin.Forms;
using DGCValidator.Views;
using Xamarin.Essentials;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.Vaccinregler.ValueSet;

namespace DGCValidator.ViewModels
{
    public class UpdateViewModel
    {
        public UpdateViewModel()
        {
        }

        public String CurrentVersion
        {
            get { return VersionTracking.CurrentVersion; }
        }
        public String LatestVersion
        {
            get { return App._latestVersion; }
        }
    }
}
