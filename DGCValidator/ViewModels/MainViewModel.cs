using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.DGC.V1;
using DGCValidator.Views;
using Xamarin.Forms;

namespace DGCValidator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        String _resultText;
        bool _resultOk;
        SignatureModel _signature;
        SubjectModel _subject;
        ObservableCollection<ICertModel> _certificates;

        private ICommand scanCommand;
        private ICommand settingsCommand;
        private ICommand aboutCommand;

        public MainViewModel()
        {
            _subject = new SubjectModel();
            _certificates = new ObservableCollection<ICertModel>();
        }

        public ICommand SettingsCommand => settingsCommand ??
                (settingsCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
                }));

        public ICommand AboutCommand => aboutCommand ??
                (aboutCommand = new Command(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
                }));

        public ICommand ScanCommand
        {
            get
            {
                return scanCommand ??
                (scanCommand = new Command(async () => await Scan()));


            }
        }

        private async Task Scan() {
            try
            {
                Clear();

                var scanner = DependencyService.Get<IQRScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    UpdateFields(result);
                }
            }
            catch (Exception ex)
            {
                ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                IsResultOK = false;
            }
        }

        private void UpdateFields(String scanResult)
        {
            try
            {
                if (scanResult != null)
                {
                    var result = VerificationService.VerifyData(scanResult);
                    if (result != null)
                    {
                        SignedDGC proof = result;
                        if (proof != null)
                        {
                            Subject = new Models.SubjectModel
                            {
                                Name = proof.Dgc.Sub.Fn+" "+proof.Dgc.Sub.Gn,
                                ConvertDateOfBirth = proof.Dgc.Sub.Dob,
                                Identifier = proof.Dgc.Sub.Id[0].I
                            };
                            Signature = new Models.SignatureModel
                            {
                                IssuedDate = proof.IssuedDate,
                                ExpirationDate = proof.ExpirationDate,
                                IssuerCountry = proof.IssuingCountry
                            };

                            bool vaccinated = false;
                            if (proof.Dgc.Vac != null)
                            {
                                foreach (Vac vac in proof.Dgc.Vac)
                                {
                                    AddCertificate(new Models.VaccineCertModel
                                    {
                                        Type = Models.CertType.VACCINE,
                                        Adm = vac.Adm,
                                        Aut = vac.Aut,
                                        Cou = vac.Cou,
                                        Dat = vac.Dat,
                                        Dis = vac.Dis,
                                        Lot = vac.Lot,
                                        Mep = vac.Mep,
                                        Seq = vac.Seq,
                                        Tot = vac.Tot,
                                        Vap = vac.Vap
                                    });
                                    vaccinated = true;
                                }

                            }
                            bool tested = false;
                            if (proof.Dgc.Tst != null)
                            {
                                foreach (Tst tst in proof.Dgc.Tst)
                                {
                                    AddCertificate(new Models.TestCertModel
                                    {
                                        Type = Models.CertType.TEST,
                                        Cou = tst.Cou,
                                        Dis = tst.Dis,
                                        Dtr = Models.TestCertModel.ConvertFromSecondsEpoc(tst.Dtr),
                                        Dts = Models.TestCertModel.ConvertFromSecondsEpoc(tst.Dts),
                                        Fac = tst.Fac,
                                        Ori = tst.Ori,
                                        Res = tst.Res,
                                        Typ = tst.Typ,
                                        Tma = tst.Tma
                                    });
                                    tested = true;
                                }
                            }
                            bool recovered = false;
                            if (proof.Dgc.Rec != null)
                            {
                                foreach (Rec rec in proof.Dgc.Rec)
                                {
                                    AddCertificate(new Models.RecoveredCertModel
                                    {
                                        Type = Models.CertType.RECOVERED,
                                        Cou = rec.Cou,
                                        Dat = rec.Dat,
                                        Dis = rec.Dis
                                    });
                                    recovered = true;
                                }
                            }

                            if(vaccinated)
                            {
                                ResultText = AppResources.VaccinatedText;
                                IsResultOK = true;
                            }
                            else if(tested)
                            {
                                ResultText = AppResources.TestedText;
                                IsResultOK = true;
                            }
                            else if (recovered)
                            {
                                ResultText = AppResources.RecoveredText;
                                IsResultOK = true;
                            }
                            else
                            {
                                ResultText = AppResources.MissingDataText;
                                IsResultOK = false;
                            }

                        }
                        else
                        {
                            ResultText = AppResources.ErrorReadingText;
                            IsResultOK = false;

                        }
                    }
                    else
                    {
                        ResultText = AppResources.ErrorReadingText + ", " + scanResult;
                        IsResultOK = false;

                    }
                }
            }
            catch (Exception ex)
            {
                ResultText = AppResources.ErrorReadingText + ", " + ex.Message;
                IsResultOK = false;
            }
        }

        public String ResultText
        {
            get { return _resultText; }
            set
            {
                _resultText = value;
                OnPropertyChanged();
            }
        }

        public bool IsResultOK
        {
            get { return _resultOk; }
            set
            {
                _resultOk = value;
                OnPropertyChanged();
            }
        }

        public SignatureModel Signature
        {
            get { return _signature; }
            set
            {
                _signature = value;
                OnPropertyChanged();
            }
        }

        public SubjectModel Subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ICertModel> Certs
        {
            get { return _certificates; }
            set
            {
                _certificates = value;
                OnPropertyChanged();
            }
        }
        public void AddCertificate(ICertModel cert)
        {
            _certificates.Add(cert);
            cert.CreateHeaderAndInfo();
            OnPropertyChanged("Certs");
        }
        public void Clear()
        {
            _resultText = null;
            if (_signature != null)
            {
                _signature.Clear();
            }
            if (_subject != null)
            {
                _subject.Clear();
            }
            if (_certificates != null)
            {
                _certificates.Clear();
            }
            OnPropertyChanged("ResultText");
            OnPropertyChanged("Signature");
            OnPropertyChanged("Subject");
            OnPropertyChanged("Certs");
        }

    }

    public class LabelColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return ((bool)value?Color.Green:Color.Red);
            }
            return Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LabelVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                return (bool)value;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}