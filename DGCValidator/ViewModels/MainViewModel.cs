using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.DGC;
using DGCValidator.Services.DGC.V1;
using DGCValidator.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        String _resultText;
        bool _resultOk = false;
        SignatureModel _signature;
        SubjectModel _subject;
        bool _hasVaccination = false;
        bool _hasTest = false;
        bool _hasRecovered = false;
        ObservableCollection<object> _certs;

        private ICommand scanCommand;
        private ICommand settingsCommand;
        private ICommand aboutCommand;
        private ICommand scanPageCommand;

        public MainViewModel()
        {
            _subject = new SubjectModel();
            _certs = new ObservableCollection<object>();
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current, "Scan", async (sender) =>
            {
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                // await Application.Current.MainPage.Navigation.PopAsync();
                scanCommand = new Command(async () => await Scan());
            });
            MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current, "Cancel", async (sender) =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
                // await Application.Current.MainPage.Navigation.PopAsync();
               //await Scan();
            });
        }

        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

        public ICommand SettingsCommand => settingsCommand ??
                (settingsCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
                }));

        public ICommand AboutCommand => aboutCommand ??
                (aboutCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
                }));

        public ICommand ScanPageCommand => scanPageCommand ??
        (aboutCommand = new Command(async () =>
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ScanPage());
        }));

        //public ICommand ScanCommand => scanCommand ??
        //        (scanCommand = new Command(async () =>
        //        {
        //            await Application.Current.MainPage.Navigation.PushAsync(new ScanPage());
        //        }));

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand ??
                (scanCommand = new Command(async () => await Scan()));
            }
        }

        public async Task Scan() {
            try
            {
                //Clear();

                var scanner = DependencyService.Get<IQRScanningService>();
                var result = await scanner.ScanAsync();

                if( result !=null)
                {
                    ResultViewModel resultModel = new ResultViewModel();
                    resultModel.UpdateFields(result);
                    ResultPage resultPage = new ResultPage();
                    resultPage.BindingContext = resultModel;
                    await Application.Current.MainPage.Navigation.PushModalAsync(resultPage);
                }
            }
            catch (Exception ex)
            {
               // ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                //IsResultOK = false;
            }
        }

       

    }

    
}