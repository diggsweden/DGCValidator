using System.Collections.ObjectModel;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.ViewModels;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.IO;

namespace DGCValidator.Views
{
    public partial class DebugPage : ContentPage
    {
        public DebugPage()
        {
        }
        public DebugPage(string dcc)
        {
            InitializeComponent();
            BindingContext = new DebugViewModel(dcc);

        }
    }
}