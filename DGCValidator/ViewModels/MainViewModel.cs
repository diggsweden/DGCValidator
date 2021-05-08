using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DGCValidator.Models;
using DGCValidator.Resources;
using DGCValidator.Services;
using DGCValidator.Services.DGC;
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
                    //App.CertificateManager.RefreshTrustListAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
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
                                Name = proof.Dgc.Nam.Fn+" "+proof.Dgc.Nam.Gn,
                                ConvertDateOfBirth = proof.Dgc.Dob,
                                Identifier = ""/*proof.Dgc.Sub.Id[0].I*/
                            };
                            Signature = new Models.SignatureModel
                            {
                                IssuedDate = proof.IssuedDate,
                                ExpirationDate = proof.ExpirationDate,
                                IssuerCountry = proof.IssuingCountry
                            };

                            bool vaccinated = false;
                            if (proof.Dgc.V != null && proof.Dgc.V.Length > 0)
                            {
                                foreach (VElement vac in proof.Dgc.V)
                                {
                                     AddCertificate(new Models.VaccineCertModel
                                    {
                                        Type = Models.CertType.VACCINE,
                                        Tg = CodeMapperUtil.GetDiseaseAgentTargeted(vac.Tg),
                                        Vp = CodeMapperUtil.GetVaccineOrProphylaxis(vac.Vp),
                                        Mp = CodeMapperUtil.GetVaccineMedicalProduct(vac.Mp),
                                        Ma = CodeMapperUtil.GetMarketingAuthHolder(vac.Ma),
                                        Dn = vac.Dn,
                                        Sd = vac.Sd,
                                        Dt = vac.Dt,
                                        Co = vac.Co,
                                        Is = vac.Is,
                                        Ci = vac.Ci
                                    });
                                    vaccinated = true;
                                }

                            }
                            bool tested = false;
                            if (proof.Dgc.T != null && proof.Dgc.T.Length > 0)
                            {
                                foreach (TElement tst in proof.Dgc.T)
                                {
                                    AddCertificate(new Models.TestCertModel
                                    {
                                        Type = Models.CertType.TEST,
                                        Tg = tst.Tg.ToString(),
                                        Tt = tst.Tt,
                                        Nm = tst.Nm,
                                        Sc = tst.Sc, /*Models.TestCertModel.ConvertFromSecondsEpoc(tst.Sc),*/
                                        Dr = tst.Dr, /*Models.TestCertModel.ConvertFromSecondsEpoc(tst.Dr),*/
                                        Tr = CodeMapperUtil.GetTestResult(tst.Tr),
                                        Tc = tst.Tc,
                                        Co = tst.Co,
                                        Is = tst.Is,
                                        Ci = tst.Ci
                                    });
                                    tested = true;
                                }
                            }
                            bool recovered = false;
                            if (proof.Dgc.R != null && proof.Dgc.R.Length > 0)
                            {
                                foreach (RElement rec in proof.Dgc.R)
                                {
                                    AddCertificate(new Models.RecoveredCertModel
                                    {
                                        Type = Models.CertType.RECOVERED,
                                        Tg = CodeMapperUtil.GetDiseaseAgentTargeted(rec.Tg),
                                        Fr = rec.Fr,
                                        Co = rec.Co,
                                        Is = rec.Is,
                                        Df = rec.Df,
                                        Du = rec.Du,
                                        Ci = rec.Ci
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