using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.DGC;
using DGCValidator.Services.DGC.V1;
using DGCValidator.Services.Vaccinregler.ValueSet;
using DGCValidator.Views;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class DebugViewModel : BaseViewModel
    {
        private ICommand backCommand;
        private string _jsonText;

        public DebugViewModel()
        {
        }
        public DebugViewModel(string dcc)
        {
            _jsonText = dcc;
        }
        public ICommand BackCommand => backCommand ??
        (backCommand = new Command(async () =>
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }));
        public String JsonText
        {
            get { return _jsonText; }
            set
            {
                _jsonText = value;
                OnPropertyChanged();
            }
        }
    }

}

