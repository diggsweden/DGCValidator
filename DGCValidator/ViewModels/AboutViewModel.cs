using System;
using System.Windows.Input;
using Xamarin.Forms;
using DGCValidator.Views;

namespace DGCValidator.ViewModels
{
    public class AboutViewModel
    {
        private Command backCommand;

        public AboutViewModel()
        {
     
        }

        public ICommand BackCommand => backCommand ??
        (backCommand = new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new MainPage());
        }));
    }
}
